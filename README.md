# Shakespearean Pokemon Search Engine
A funny way to retrieve an alternative Pokemon description

# Run app in Development environment
In order to prepare the enviroment you have to:
1. Clone git repository on your local folder 
2. Prepare Backend solution:
    - Open WebAPI folder
    - Open WebAPI.sln with Visual Studio
    - Run the project
    - On the url (https://localhost:44390/swagger) it is possible to check the available endpoint
3. Prepare Frontend solution:
    - Open Visual Studio Code an select the folder _react-app_
    - If not present, install node (run `node -v` on the terminal to check)
    - In order to run the app run `npm start` on the terminal
    - If some package is missing using `npm install` to retrieve it

# Run app in Test environment
Assuming you have a dedicated Test environment (physical machine or Iaas virtual machine) with a Windows OS, you have to create 2 different sites:
1. Backend site:
    - There are different approach to get a compiled built version of the application. The most common is to use the _Publish_ utility of Visual Studio, but, you can also build your own Powershell script.
    - Create on test machine IIS a dedicated application with a dedicated pool (check if the machine has all the necessary IIS dependecies installed)
    - Copy all the file of the build version in the IIS site folder
2. Frontend site: 
    - Set _SERVER_URL_ parameter in configuration.json file (it contains the url of the backend site)
    - Run `npm run build` in the terminal in order to generate a build version of the site
    - Deploy the folder on a dedicated site previously created in IIS (for all the details you can read something here (https://www.c-sharpcorner.com/article/how-to-deploy-react-application-on-iis-server/))

# Run app in Production environment
To deploy in Production environment you can follow the same steps described for the Test environment, targeting Prod environment.
Never the less we can imagine you want to use a Cloud environment to host your app.
Assuming you are using Azure, you can create 2 different App Service on Azure and each of them will host the backend and the frontend application.
Given that, you have to follow the following step:
1. Backend application:
    - Create a build pipeline on Azure DevOps, using template ot YAML configuration, in order to create a built version of the solution, targeting the release Git branch
    - Run the pipeline and generate the package
    - Create a release pipeline, targeting the dedicated App Service, in order to deploy there the built package create in the previous step
    - Run the realease pipeline, using the package created for the backend release
    - Pay attention that you can handle _appsettings.json_ values directly on Azure Configuration page
2. Frontend application:
    - Create another build pipeline on Azure DevOps, in order to create a built version of the frontend site. Basically this pipeline should execute a `npm run build` and store the result in a folder on the cloud    
    - Run the pipeline and generate the package
    - Create a release pipeline, targeting the dedicated App Service, in order to deploy there the built package create in the previous step. 
    - Run the realease pipeline, using the package created for the frontend release
    - Pay attention that you can handle _configuration.json_ values directly on Azure Configuration page

# Chosen architecture
The frontend architecture is based on a simple structure in React (don't blame me for its semplicity, it is my first time on React), which has basically a single component (SearchComponent.js), that handles the state of the search and manages the call to the backend.
The site is a responsive Single-Page-Application.
The backend is a simple .NET Core solution which exposes a single API. It is devided in 3 simple layer (using a top-down approach you can find _WebAPIEndpoints_, that manages the endpoint, _BusinessLogic_, that contains all the logic to retrieve information, and _ViewModels_ that has all ViewModel classes.

# Testing approach
I created a simple NUnit test suite for the backend part, testing the endpoint and the business logic.
Test on endpoint are based mainly on the correctness of the call and the response type. For this reason in these tests there is no need to really invoke Buisness Logic and it is possible to mock it.
On the contrary, test on business logic perform real calls to external API to retrieve data and verify the correctness of the whole process.

Here below you can find the test implmented:
1. Endpoint:
    - Response Type test
    - Test on data retrieved
    - Test on methods called during API invokation
2. Endpoint:
    - Response Type test
    - Test on data retrieved (tre cases: good input, wrong input, no input)

# Any other notable design decisions that you made
Two intersting libraries are included in the project:
1. Swagger: 
    - It creates backend documentation automatically
    - It is possible to use it for the first simple tests during development
2. Polly: 
    - It is useful to handle resilency on extenal API call

# Anything you’d like to have implemented, but were not able to complete within the timeframe
Probably would have been interesting to handle the creation of Docker image for the backend site. If I had had more time, probably I would have created a Docker instance of the application (using Visual Studio), I would have included it in a Azure Container Registry and I would have deployed it on an Azure instance. In this simple case this approch would't have speeded up the solution, but it would have been interesting in order to experiment on it.
