// See https://aka.ms/new-console-template for more information
using PortfolioT.DataBase;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.Services.GitService;
using PortfolioT.Services.LibService.Parsers;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using TestS;
using TestS.Commons;
using TestS.DB;

ElibTest test = new ElibTest();
await test.run();




