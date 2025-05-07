using PortfolioT.Analysis.Models;
using PortfolioT.Analysis.Models.httpResponse;
using PortfolioT.Analysis.Models.XmlCommon;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;

namespace PortfolioT.Analysis
{
    public class SonarQubeScanner
    {
        private readonly string GRADLE_PLUGIN = "id \"org.sonarqube\" version \"6.0.1.5171\"";
        private readonly string URL_SONAR_SERVER = "http://localhost:9000";
        private readonly string SONAR_TOKEN = "squ_4decc5059e263b89293f2890e739b8557753d591";
        private readonly int count_req = 3;

        private readonly float max_scope = 15;
        private readonly float best_scope = 3;

        private HttpClient httpClient;

        public SonarQubeScanner(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> analisysCode(
            string language,
            string fullName,
            string path_dir)
        {
            bool check = false;
            Stopwatch stopwatch = new Stopwatch();
            //Console.WriteLine($"Start analisys: {fullName}");
            stopwatch.Start();
            switch (language.ToLower())
            {
                case "java":
                    check = await analisysJava(fullName, path_dir);
                    break;
                case "c#":
                    check = await analisysCsharp(fullName, path_dir);
                    break;
                default:
                    check = await analisysOther(fullName, path_dir);
                    break;
            }
            stopwatch.Stop();
            //Console.WriteLine($"Name: {fullName} Time: {stopwatch.ElapsedMilliseconds / 1000}");
            return check;

        }
        public async Task<AnalisysResponse> getResultAnalisys(int id, string name)
        {
            string str = "";
            MetricResponse? metrics;
            AnalisysResponse analisys = new AnalisysResponse(id);

            using var request = new HttpRequestMessage(HttpMethod.Get, $"{URL_SONAR_SERVER}/api/measures/component?additionalFields=metrics&component={name}&metricKeys=software_quality_reliability_rating,software_quality_maintainability_rating,software_quality_security_rating,ncloc");
            request.Headers.Add("Authorization", $"Bearer {SONAR_TOKEN}");
            
            using HttpResponseMessage response = await httpClient.SendAsync(request);
            
            bool againLoop = !response.IsSuccessStatusCode;
            int i = 0;

            while (againLoop && i < count_req)
            {
                i++;
                using (var new_request = new HttpRequestMessage(HttpMethod.Get, $"{URL_SONAR_SERVER}/api/measures/component?additionalFields=metrics&component={name}&metricKeys=software_quality_reliability_rating,software_quality_maintainability_rating,software_quality_security_rating,ncloc"))
                {
                    new_request.Headers.Add("Authorization", $"Bearer {SONAR_TOKEN}");
                    using (HttpResponseMessage again_response = await httpClient.SendAsync(new_request))
                    {
                        if (!again_response.IsSuccessStatusCode)
                        {
                            againLoop = true;
                            await Task.Delay(2000);
                            continue;
                        }

                        str = await response.Content.ReadAsStringAsync();
                        metrics = JsonSerializer.Deserialize<MetricResponse>(str);
                        if (metrics != null && metrics.measures.Count > 0)
                        {
                            againLoop = false;
                            await Task.Delay(1000);
                            break;
                        }
                    }
                }
                
            }

            if(againLoop)
            {
                analisys.comments = "Ошибка при проведении анализа";
                return analisys;
            }

            str = await response.Content.ReadAsStringAsync();
            metrics = JsonSerializer.Deserialize<MetricResponse>(str);

            List<string> comments = new List<string>();
            float scope = 0;
            bool checkNloc = false;
            foreach (Measure measure in metrics.measures)
            {
                if (measure.metric.Equals("ncloc"))
                    checkNloc = true;
                else
                {
                    float scope_metric = float.Parse(measure.value.Replace('.', ','));
                    switch (measure.metric)
                    {
                        case "software_quality_reliability_rating":
                            analisys.scope_reability = scope_metric;
                            break;
                        case "software_quality_maintainability_rating":
                            analisys.scope_maintability = scope_metric;
                            break;
                        case "software_quality_security_rating":
                            analisys.scope_security = scope_metric;
                            break;

                    }
                    scope += float.Parse(measure.value.Replace('.', ','));
                    comments.Add(measureMean(measure.metric, float.Parse(measure.value.Replace('.', ','))));
                }
                

            }
            if(!checkNloc)
            {
                analisys.comments = "Ошибка при проведении анализа";
                analisys.scope_cof = 0;
                return analisys;
            }
            float result_conf = MathF.Round(1-(scope-best_scope)/(max_scope-best_scope),3);
            analisys.scope_cof = result_conf;
            analisys.comments = string.Join('\n',comments);
            return analisys;
        }

        public async Task<bool> deleteProjects(string name)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{URL_SONAR_SERVER}/api/projects/delete?project={name}");
            request.Headers.Add("Authorization", $"Bearer {SONAR_TOKEN}");
            using HttpResponseMessage response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        private string measureMean(string name, float value)
        {
            switch (name)
            {
                case "software_quality_security_rating":
                    if (value > 2)
                        return "Проблемы с безопасностью";
                    return "";
                case "software_quality_reliability_rating":
                    if (value > 2)
                        return "Проблемы с переиспользованием";
                    return "";
                case "software_quality_maintainability_rating":
                    if (value > 2)
                        return "Проблемы с надежностью";
                    return "";
                default:
                    return "";
            }
        }
        private async Task<bool> analisysJava(string fullName, string path_dir)
        {
            string? path_maven_conf = searchConfig(path_dir, "pom.xml");
            string? path_gradle_conf = searchConfig(path_dir, "build.gradle");
            if (path_maven_conf != null)
            {
                checkXml(path_maven_conf);
                FileInfo file = new FileInfo(path_maven_conf);
                return await RunMavenCommands(file.DirectoryName, fullName);
            }
            else if (path_gradle_conf != null)
            {
                await checkGradle(path_gradle_conf);
                FileInfo file = new FileInfo(path_gradle_conf);
                return await RunGradleCommands(file.DirectoryName, fullName);

            }
            else
            {
                return await RunJavaCommands(path_dir, fullName);
            }
        }


        private async Task<bool> analisysCsharp(string fullName, string path_dir)
        {
            string csProjPath = searchConfig(path_dir, "*.csproj");
            string path_sln = searchConfig(path_dir, "*.sln");
            if (csProjPath == null || path_sln == null)
                return false;
            FileInfo file = new FileInfo(path_sln);

            if (await checkNetCoreConf(csProjPath))
                return await RunNetCoreCommands(fullName, file.DirectoryName);
            else
                return await RunNetFrameworkCommands(fullName, file.DirectoryName);

        }
        private async Task<bool> RunNetCoreCommands(string fullName, string path_dir)
        {
            string cdCmd = commandCD(path_dir);
            string startCmd = $"dotnet sonarscanner begin /k:\"{fullName}\" /d:sonar.host.url=\"{URL_SONAR_SERVER}\"  /d:sonar.token=\"{SONAR_TOKEN}\"";
            string buildCmd = $"dotnet build";
            string analisysCmd = $"dotnet sonarscanner end /d:sonar.token=\"{SONAR_TOKEN}\"";
            string resCmd = $"{cdCmd} && {startCmd} && {buildCmd} && {analisysCmd}";
            return await RunCommand(resCmd);
        }

        private async Task<bool> RunNetFrameworkCommands(string fullName, string path_dir)
        {
            string cdCmd = commandCD(path_dir);
            string startCmd = $"SonarScanner.MSBuild.exe begin /k:\"{fullName}\" /d:sonar.host.url=\"{URL_SONAR_SERVER}\" /d:sonar.token=\"{SONAR_TOKEN}\"";
            string buildCmd = $"MsBuild.exe /t:Rebuild";
            string analisysCmd = $"SonarScanner.MSBuild.exe end /d:sonar.token=\"{SONAR_TOKEN}\"";
            string resCmd = $"{cdCmd} && {startCmd} && {buildCmd} && {analisysCmd}";
            return await RunCommand(resCmd);
        }

        private async Task<bool> analisysOther(string fullName, string path_dir)
        {
            string? path_maven_conf = searchConfig(path_dir, "pom.xml");
            string? path_gradle_conf = searchConfig(path_dir, "build.gradle");
            string? path_sln_conf = searchConfig(path_dir, "*.sln");

            if (path_maven_conf != null || path_gradle_conf != null)
                return await analisysJava(fullName, path_dir);
            else if (path_sln_conf != null)
                return await analisysCsharp(fullName, path_dir);

            string cdCmd = commandCD(path_dir);
            string analisysCmd = $"sonar-scanner.bat -D \"sonar.projectKey={fullName}\" -D\"sonar.sources=./\" -D \"sonar.host.url={URL_SONAR_SERVER}\" -D \"sonar.token={SONAR_TOKEN}\" -D \"sonar.exclusions=**/*.java\"";
            string resCmd = $"{cdCmd} && {analisysCmd}";
            return await RunCommand(resCmd);
        }

        private string commandCD(string path)
        {
            return $"cd {path}";
        }

        private async Task<bool> RunCommand(string command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command} || echo Status: BAD_ANALISYS",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                await process.WaitForExitAsync();

                if (!string.IsNullOrEmpty(error))
                {
                    string[] lines = error.Split('\n');
                    if (lines[lines.Length-1].Contains("Status: BAD_ANALISYS"))
                        return false;
                }

                return true;
            }
        }

        private async Task<bool> RunMavenCommands(string path, string fullName)
        {
            string cdCmd = commandCD(path);
            string mvnCmd = $"mvn clean verify sonar:sonar -D sonar.projectKey={fullName} -D sonar.projectName={fullName} -D sonar.host.url={URL_SONAR_SERVER} -D sonar.token={SONAR_TOKEN}";
            string resCmd = $"{cdCmd} && {mvnCmd}";
            return await RunCommand(resCmd);
        }
        private async Task<bool> RunGradleCommands(string path, string fullName)
        {
            string cdCmd = commandCD(path);
            string buildCmd = "gradle build";
            string gradleCmd = $"gradle sonar -D sonar.projectKey={fullName} -D sonar.projectName={fullName} -D sonar.host.url={URL_SONAR_SERVER} -D sonar.token={SONAR_TOKEN}";
            string resCmd = $"{cdCmd} && {buildCmd} && {gradleCmd}";
            return await RunCommand(resCmd);
        }
        private async Task<bool> RunJavaCommands(string path, string fullName)
        {
            string cdCmd = commandCD(path);
            string searchJavaCmd = "dir /s /B *.java > sources.txt";
            string compileJavaCmd = "javac -d GenClasses @sources.txt";
            string analisysCmd = $"sonar-scanner.bat -D \"sonar.projectKey={fullName}\" -D \"sonar.sources=.\" -D \"sonar.host.url={URL_SONAR_SERVER}\" -D \"sonar.token={SONAR_TOKEN}\" -D sonar.java.binaries=\"./GenClasses\"";
            string resCmd = $"{cdCmd} && {searchJavaCmd} && {compileJavaCmd} && {analisysCmd}";
            return await RunCommand(resCmd);
        }
        private string? searchConfig(string path_dir, string pattern)
        {
            DirectoryInfo dir = new DirectoryInfo(path_dir);
            string[] dirs = Directory.GetFiles(dir.FullName, pattern, SearchOption.AllDirectories);
            if (dirs.Length == 0)
                return null;
            else
                return dirs[0];
        }
        private void checkXml(string path)
        {
            List<XmlProperty> properties = new List<XmlProperty>()
            { new XmlProperty("java.version", "17"),
                new XmlProperty("maven.compiler.source", "17"),
                new XmlProperty("maven.compiler.target", "17"),
                new XmlProperty("project.build.sourceEncoding", "UTF-8") };
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlElement? xRoot = xDoc.DocumentElement;
            string xmlns = xRoot.Attributes["xmlns"].Value;
            XmlElement? propNode = searchNode(xRoot, "properties");
            if (propNode == null)
            {
                propNode = xDoc.CreateElement("properties");

                XmlAttribute xmlAttribute = xDoc.CreateAttribute("xmlns");
                XmlText xmlnsText = xDoc.CreateTextNode(xmlns);
                xmlAttribute.AppendChild(xmlnsText);

                foreach (XmlProperty prop_name in properties)
                {
                    XmlElement prop = xDoc.CreateElement(prop_name.name);
                    XmlText xmlText = xDoc.CreateTextNode(prop_name.value);
                    prop.AppendChild(xmlText);
                    propNode.AppendChild(prop);
                }
                propNode.Attributes.Append(xmlAttribute);
                xRoot.InsertAfter(propNode, searchNode(xRoot, "licenses"));
            }
            else
            {
                List<XmlProperty> clone_props = new List<XmlProperty>(properties);
                foreach (XmlElement node in propNode)
                {
                    XmlProperty? prop = clone_props.SingleOrDefault(pr => pr.name.Equals(node.Name));
                    if (prop != null)
                    {
                        node.InnerText = prop.value;
                        clone_props.RemoveAll(pr => pr.name.Equals(node.Name));
                    }

                }
                if (clone_props.Count != 0)
                {

                    foreach (XmlProperty prop_name in clone_props)
                    {
                        XmlAttribute xmlAttribute = xDoc.CreateAttribute("xmlns");
                        XmlText xmlnsText = xDoc.CreateTextNode(xmlns);
                        xmlAttribute.AppendChild(xmlnsText);

                        XmlElement prop = xDoc.CreateElement(prop_name.name);
                        XmlText xmlText = xDoc.CreateTextNode(prop_name.value);

                        prop.AppendChild(xmlText);
                        prop.Attributes.Append(xmlAttribute);

                        propNode.AppendChild(prop);
                    }
                }
            }
            xDoc.Save(path);
        }
        private XmlElement? searchNode(XmlElement root, string name)
        {
            foreach (XmlElement node in root)
                if (node.Name.Equals(name))
                    return node;
            return null;

        }

        private async Task<bool> checkGradle(string path)
        {
            string[] lines = await File.ReadAllLinesAsync(path);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("plugins"))
                {
                    for (int j = i; ; j++)
                    {
                        if (lines[j].Contains("id \"org.sonarqube\""))
                        {
                            return true;
                        }
                        if (lines[j].Contains("}"))
                        {
                            lines[j] = lines[j].Replace("}", $"\n\t{GRADLE_PLUGIN} \n}}");
                            break;
                        }
                    }
                }
            }
            await File.WriteAllLinesAsync(path, lines);
            return true;
        }
        private async Task<bool> checkNetCoreConf(string path)
        {

            string[] lines = await File.ReadAllLinesAsync(path);
            string patternProperty = @"<TargetFramework\w*>";
            string patternNet = @"net\d{1}\.\d{1}";
            for (int i = 0; i < lines.Length; i++)
            {
                if (Regex.IsMatch(lines[i], patternProperty, RegexOptions.IgnoreCase))
                {
                    if (Regex.IsMatch(lines[i], patternNet, RegexOptions.IgnoreCase))
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
    }
}
