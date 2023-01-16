# SalveApp Web Architecture
 
The solution contains ASP.NET Core Razor-based Web Application which is structured into the following components:
 
[Architecture]
![Architecture](/Diagrams/Architecture.png)
 
* **Clinics Web Page** (\SalveApp.Clinics.Web\Pages)- Contains the web application for browsing patient data.
* **ASP.NET Core Razor Web Application** (SalveApp.Clinics.Web)- Contains the core of web application logic and wiring up all the components together.
* **Cache Mechanism** (SalveApp.Clinics.Core.Cache)- This is a bespoke component that holds the patients' data response for a given number of minutes.
* **CSV DataLoader** (SalveApp.Clinics.Core\Services\ICSVDataLoader.cs) To parse and load the CSV data files from the given location.
 
## General Flow
 
As per the diagram above, generally flow works as follows:
1. Browse patient data: The user browses the clinic data web page and selects his preferred clinic data to view related patients.
2. Request is received by the Web application and which checks for the cached results in the local cache.
3. If results for the given clinics and patients are not found locally then it loads and refreshes the cache results.
4. Then latest results are cached for the configured amount of time in minutes.
 
 
#  Setting Up and Running the Application
 
1. **Prerequisite:** .Net 6.0
2. Application can be run via the given RunApp.ps1 PowerShell script.
   It will run all the tests, and the application will start running on a local Kestrel server.
    [RunApp.ps1]
    ![RunApp.ps1](/Diagrams/RunApp-ps1.png)
3. Launch the application by default running at [https://localhost:5001/](https://localhost:5001/)
   [App Front Page]
    ![App Front Page](/Diagrams/AppFPage.png)
4.  Selecting a clinic will fetch back the corresponding patient data.
   [Selecting a clinic]
    ![Selecting a clinic](/Diagrams/AppSPage.png)
5. Patient data can be sorted via attributes like name or date of birth.
    [Sorting]
    ![Sorting](/Diagrams/AppSPageSort.png)
6. Large data set will be paged via configurable page sized (currently set to 50 records).
    [Paging]
    ![Paging](/Diagrams/AppSPagePaging.png)
## Features
1. Decorator pattern-based cache mechanism.
2. Clear, separated by concerns of modular design.
 
   [Modular Design]
   ![Modular Design](/Diagrams/ModDesign.png)
3. Separated set of Integrations and Unite Tests suites, which tests the whole application inside out.
    [Tests suites]![Tests suites](/Diagrams/Tests.png) 
4. Cache timing and data paging size could be configured via the given config file 
    [configs] ![configs](/Diagrams/Configs.png) 
## Areas for improvements

1. Test coverage is just a starting point, it's not complete and comprehensive.
2. UI is basic and could improve with proper UX input, seems a bit flaky at times etc paging buttons playing up requires more thorough testing. 
4. There are lots of simple assumptions that have been made about the size, volume, location and frequency etc of CSV data files and its handling. 
5. In real-time implementation these assumptions will be clarified for a better and real-time robust design i.e processing the CSV files in a background service and dumping the results into a database store.
6. Load, stress and performance testing in the production-like environment will significantly improve the application before going live.
7. There could be improvement with more logging, error handling, alert and monitoring, parallelism and cache etc and so on.
8. Peer code review and design discussions always yield better results.
