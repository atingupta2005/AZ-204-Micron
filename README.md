# Lab 04: Constructing a polyglot data solution
## Create an Azure SQL Database server resource
- polysqlsrvr[yourname]
- testuser
- TestPa55w.rd
- Networking - Allow Azure services and resources to access this server
-
## Create an Azure Cosmos DB account resource
- polycosmos[yourname]
- Core (SQL)
- Apply Free Tier Discount: Do Not Apply
- Account Type section, select Non-Production
- Location drop-down list, select the (US) East US region
- Multi-region Writes section, select Disable.
- Copy PRIMARY CONNECTION STRING from Keys

## Create an Azure Storage account resource
- polystor[yourname]
- In the Replication drop-down list, select Locally-redundant storage (LRS).
- In the Location drop-down list, select the (US) East US region.

## Import and validate data
- polystor[yourname]
- container - images
- Public access level drop-down list, select Blob (anonymous read access for blobs only).
- Copy URL of container from Properties
- Upload all files from F:\Allfiles\Labs\04\Starter\Images

## Upload an SQL .bacpac file
- Open storage Account
- + Container - named - databases
- Public access level drop-down list, select Private (no anonymous access)
- Upload F:\Allfiles\Labs\04\Starter\AdventureWorks.bacpac to the Container

## Import an SQL database
- Open SQL Server - polysqlsrvr[yourname]
- From the SQL server blade, select Import database
- Username: testuser
- Password: TestPa55w.rd

## Use an imported SQL database
- Open polysqlsrvr[yourname]
- Firewalls and virtual networks \ Add Client IP
- Open database - AdventureWorks
- From the SQL database blade, find the Settings section from the blade, and then select the Connection strings link
- In the Connection strings pane, record the value in the ADO.NET (SQL Authentication) text box. You’ll use this value later in this lab.
- Update password in Connection string
- Open  Query editor
  - In the Login text box, enter testuser.
  - In the Password text box, enter TestPa55w.rd
```
SELECT * FROM AdventureWorks.dbo.Models
SELECT * FROM AdventureWorks.dbo.Products
```

## Open and configure a .NET web application
- Open Visual Studio Code
- Open Folder -F:\Allfiles\Labs\04\Starter\AdventureWorks
- Open Terminal
- dotnet build
- Update connection strings in AdventureWorks.Web\appsettings.json

## Migrating SQL data to Azure Cosmos DB
- cd .\AdventureWorks.Migrate\
- dotnet build
- Update connection string variables in program.cs
- dotnet run

## Run App
- cd .\AdventureWorks.Web\
- dotnet run
- In the open browser window, browse to the currently running web application (http://localhost:5000)
- In the web application, observe the list of models displayed from the front page.
- Find the Touring-1000 model, and then select View Details.
- From the Touring-1000 product detail page, perform the following actions:
  - In the Select options list, select Touring-1000 Yellow, 50, $2,384.07.
  - Find Add to Cart, and then observe that the checkout functionality is still disabled.


# Lab 05: Deploying compute workloads by using images and containers
## Create a VM by using the Azure Command-Line Interface (CLI)

- Open Cloud Shell\Bash
```
az vm create --resource-group ContainerCompute --name quickvm --image Debian --admin-username student --admin-password StudentPa55w.rd
az vm show --resource-group ContainerCompute --name quickvm
ipAddress=$(az vm list-ip-addresses --resource-group ContainerCompute --name quickvm --query '[].{ip:virtualMachine.network.publicIpAddresses[0].ipAddress}' --output tsv)
echo $ipAddress
ssh student@$ipAddress
uname -a
exit
```

## Create a Docker container image and deploy it to Azure Container Registry
```
mkdir ipcheck
cd ipcheck
dotnet new console --output . --name ipcheck
touch Dockerfile
code .
```

- Update Program.cs with code:
```
public class Program
{
    public static void Main(string[] args)
    {        
        // Check if network is available
        if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
        {
            System.Console.WriteLine("Current IP Addresses:");

            // Get host entry for current hostname
            string hostname = System.Net.Dns.GetHostName();
            System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry(hostname);

            // Iterate over each IP address and render their values
            foreach(System.Net.IPAddress address in host.AddressList)
            {
                System.Console.WriteLine($"\t{address}");
            }
        }
        else
        {
            System.Console.WriteLine("No Network Connection");
        }
    }
}
```

- Run code
```
dotnet run
```

- Docker File content
```
# Start using the .NET Core 3.1 SDK container image
FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine AS build

# Change current working directory
WORKDIR /app

# Copy existing files from host machine
COPY . ./

# Publish application to the "out" folder
RUN dotnet publish --configuration Release --output out

# Start container by running application DLL
ENTRYPOINT ["dotnet", "out/ipcheck.dll"]
```

## Create a Container Registry resource using gpoertal
- In the Location text box, select East US.
- In the SKU drop-down list, select Basic.

##  Open Azure Cloud Shell and store Container Registry metadata
```
az acr list
acrName=$(az acr list --query "max_by([], &creationDate).name" --output tsv)
echo $acrName
cd ipcheck
az acr build --registry $acrName --image ipcheck:latest .
```

# Deploy an Azure container instance
## Enable the admin user in Container Registry
- Open Container registry
- Enable admin user

## Automatically deploy a container image to an Azure container instance
- Create ACI named - managedcompute
- In the Number of cores drop-down list, select 2.
- In the Memory (GB) text box, enter 4.
- In the Public IP address section, select No.

## Validate that the container instance ran successfully
- Open the container and login to it


# Lab 06: Authenticating to and querying Microsoft Graph by using MSAL and .NET SDKs
## Create an Azure Active Directory (Azure AD) application registration
### Create an application registration
- From the Azure Active Directory blade, select App registrations in the Manage section.

### Enable the default client type
- In the graphapp application registration blade, select Authentication in the Manage section
- In the Authentication section, perform the following actions:
  - In the Advanced settings - Allow public client flows subsection, select Yes.

### Record unique identifiers
- On the graphapp application registration blade, select Overview.
- In the Overview section, find and record the value of the Application (client) ID text box. You’ll use this value later in the lab.
- In the Overview section, find and record the value of the Directory (tenant) ID text box. You’ll use this value later in the lab.

## Obtain a token by using the MSAL.NET library
### Create a .NET project
- Open Project in VSC
  - F:\Allfiles\Labs\06\Solution\GraphClient
- Update the _tenantId, _clientId  string constant by setting its value to the Directory (tenant) ID that you recorded earlier in this lab.

### Test application
```
dotnet run
```
- Complete steps in the browser which is opened
- Refer to the console again and notice the token

# Lab 06: Authenticating to and querying Microsoft Graph by using MSAL and .NET SDKs
## Create an Azure Active Directory (Azure AD) application registration
### Create an application registration
- Name: graphapp
- In the Redirect URI drop-down list, select Public client/native (mobile & desktop).
- In the Redirect URI text box, enter http://localhost

### Enable the default client type
- In the graphapp application registration blade, select Authentication in the Manage section.
- In the Authentication section, perform the following actions:
  - In the Advanced settings - Allow public client flows subsection, select Yes.
  - Select Save
### Record unique identifiers
Application (client) ID
Directory (tenant) ID

## Obtain a token by using the MSAL.NET library
### Create a .NET project
- Open VSC
- Open F:\Allfiles\Labs\06\Solution\GraphClient

### Test the updated application
- dotnet run

## Query Microsoft Graph by using the .NET SDK






# Lab 07: Access resource secrets more securely across services
## Create Azure resources
### Create an Azure Storage account
- Name securestor[yourname]
- Copy Access keys

### Create an Azure Key Vault
-  securevault[yourname]

### Create an Azure Functions app
- securefunc[yourname]
- Runtime stack drop-down list, select .NET. 3.1
- Operating System section, select Linux.
- Select Storage account - securestor[yourname]
- Consumption (Serverless)

## Configure secrets and identities
### Configure a system-assigned managed service identity
- Open securefunc[yourname]
- Enable System assigned identity

### Create a Key Vault secret
- Open securevault[yourname]
- In the Secrets pane, select Generate/Import.
  - Manual
  - Name: storagecredentials
  - In the Value text box, enter the storage account connection string
- Open the new secret created
- Record the value of the Secret Identifier

### Configure a Key Vault access policy
- Open securevault[yourname]
- Add access policy
  - Service Principal - securefunc[yourname]
  - In the Secret permissions drop-down list, select the GET,LIST permission.

### Create a Key Vault-derived application setting
- Open securefunc[yourname]
- From the App Service blade, select the Configuration option from the Settings section.
- From the Configuration pane, perform the following actions:
  - Select the Application settings tab, and then select New application setting.
  - In the Add/Edit application setting pop-up window, in the Name text box, enter StorageConnectionString.
  - In the Value text box, construct a value by using the following syntax
    - @Microsoft.KeyVault(SecretUri=*Secret Identifier*)
  - Sample Value - @Microsoft.KeyVault(SecretUri=https://securevaultatin.vault.azure.net/secrets/storagecredentials/b014b228596349dd8274a318f4a920ac)

## Build an Azure Functions app
### Initialize a function project
- Open VSC
  - F:\Allfiles\Labs\07\Solution\func
- Open FileParser.cs
- Update value of StorageConnectionString

### Validate the local function
```
cd F:\Allfiles\Labs\07\Solution\func
func start --build
```
- Visit - http://localhost:7071

### Deploy using the Azure Functions Core Tools
```
cd F:\Allfiles\Labs\07\Solution\func
az login
func azure functionapp publish <function-app-name>
```

###  Test the Key Vault-derived application setting
- Open securefunc[yourname]
- Open FileParser function
- Code + Test
- Test/Run
  - In the HTTP method list, select GET.
- Run

## Access Azure Blob Storage data
### Upload a sample storage blob
- Open securestor[yourname]
- + Container
- Name: drop
- Public access level drop-down list, select Blob (anonymous read access for blobs only), and then select Create.
- Open container drop
- Upload
- F:\Allfiles\Labs\07\Starter\records.json

### Pull and configure the Azure SDK for .NET
```
cd F:\Allfiles\Labs\07\Solution\func
az login
func azure functionapp publish <function-app-name>
```

- Open securefunc[yourname]
- From the App Service blade, select the Functions option from the Functions section.
- In the Functions pane, select the existing FileParser function.
- In the Function blade, select the Code + Test option from the Developer section.
- In the function editor, select Test/Run.
- In the HTTP method list, select GET
- Select Run to test the function.
- Observe the results of the test run. The output will contain the content of the $/drop/records.json blob stored in your Azure Storage account


# Lab 8 Creating a multi-tier solution by using services in Azure
## Creating an Azure App Service resource by using a Docker container image
### Create a web app by using Azure App Service resource by using an httpbin container image
- Create Web App named - httpapi[yourname]
- In the Publish section, select Docker Container.
- In the Operating System section, select Linux
- In the Linux Plan (East US) section, select Create new, enter the value ApiPlan in the Name text box, and then select OK.
- From the Docker tab, perform the following actions:
  - In the Options drop-down list, select Single Container.
  - In the Image Source drop-down list, select Docker Hub.
  - In the Access Type drop-down list, select Public.
  - In the Image and tag text box, enter kennethreitz/httpbin:latest
  - Select Review + Create.

### Test the httpbin web application
- Opem httpapi[yourname]
- Browse
- Within the web application, perform the following actions
  - Select Response formats.
  - Select GET /xml.
  - Select Try it out.
  - Select Execute.
  - Observe the value of the Response body and Response headers text boxes.
  - Observe the value of the Request URL text box.
- Open httpapi[yourname]
- In the Properties section, record the value of the URL text box.

## Build an API proxy tier by using Azure API Management
### Create an API Management resource
- Create resource - API Management
- Name: prodapi[yourname]
- In the Organization name text box, enter Contoso
- In the Pricing tier list, select Consumption (99.9 SLA, %)

### Define a new API
- Open prodapi[yourname]
- From the API Management Service blade, in the API Management section, select APIs
- In the Add a new API section, select Blank API.
  - In the Display name text box, enter HTTPBin API.
  - In the Name text box, enter httpbin-api.
  - In the Web service URL text box, enter the URL for the web app that you copied earlier in this lab.
  - Leave the API URL suffix text box empty
- From the Design tab, select Add operation.
- In the Add operation section, perform the following actions:
  - In the Display name text box, enter Get Legacy Data.
  - In the Name text box, enter get-legacy-data.
  - In the URL list, select GET.
  - In the URL text box, enter /xml.
  - Select Save
- Back from the Design tab, in the list of operations, select Get Legacy Data.
- From the Test tab, select the Get Legacy Data operation.
- In the Get Legacy Data section, select Send.
- Observe the results of the API request.
- Back from the Design tab, in the list of operations, select Get Legacy Data.
- In the Design section for the Get Legacy Data operation, find the Outbound processing tile, and then select Add policy.
- In the Add outbound policy section, select the Other policies tile.
- In the policy code editor, find the following block of XML content:
```
<outbound>
    <base />
    <xml-to-json kind="direct" apply="always" consider-accept-header="false" />
</outbound>
```
- Back from the Design tab, in the list of operations, select Get Legacy Data
- From the Test tab, select the Get Legacy Data operation
- In the Get Legacy Data section, select Send
- Observe the results of the API request
- Within the HTTP response section, perform the following actions
  - Select Trace.
  - Observe the content in the Backend and Outbound text boxes


# Lab 09: Publishing and subscribing to Event Grid events
## Create a custom Event Grid topic
- Create Event Grid Topic
- Name: hrtopic[yourname]
- Resource Group: PubSubEvents
- Event Grid Schema - select Create

## Deploy the Azure Event Grid viewer to a web app
- Create app service
- name - eventviewer[yourname]
- Publish : Docker Container
- OS: Linux
- Docker Hub
- Image: microsoftlearning/azure-event-grid-viewer:latest

#### Review: Created the Event Grid topic and a web app which is used to view the events in event grid

## Create an Event Grid subscription
### Access the Event Grid Viewer web application
- Open eventviewer[yourname]
- Copy URL
- Open URL on web and Observe the currently running Azure Event Grid viewer web application

### Create new subscription
- Open hrtopic[yourname]
- + Event Subscription
- Name: basicsub
- In the Event Schema list, select Event Grid Schema.
- In the Endpoint Type list, select Web Hook.
- Select Endpoint.
- In the Select Web Hook dialog box, in the Subscriber Endpoint text box, enter the Web App URL value that you recorded earlier, ensure it uses an https:// prefix, add the suffix /api/updates, and then select Confirm Selection.
    - Example: https://eventviewerstudent.azurewebsites.net/api/updates

### Observe the subscription validation event
- Open web app on browser
- Review the Microsoft.EventGrid.SubscriptionValidationEvent event that was created as part of the subscription creation process.
- Select the event and review its JSON content.

### Record subscription credentials
- Open hrtopic[yourname]
- On the Event Grid Topic blade, record the value of the Topic Endpoint field
- In the Settings category\Access keys link\ record the value of the Key 1

#### Review: Created a new subscription, validated its registration, and then recorded the credentials required to publish a new event to the topic

## Publish Event Grid events from .NET
### Create a .NET project
- Open VSC
- Open F:\Allfiles\Labs\09\Solution\EventPublisher
- Open Terminal
-  dotnet run

### Observe published events
- Open web app
- Review the Employees.Registration.New events that were created by your console application.
- Select any of the events and review its JSON content.

#### Published new events to your Event Grid topic using a .NET console application

# Lab 10: Asynchronously processing messages by using Azure Queue Storage
## Create Azure resources
### Create a Storage account
- Resource Group: AsyncProcessor
- Name: asyncstor[yourname]
- Copy Connection string

## Configure the Azure Storage SDK in a .NET project
### Create a .NET project
- Open VSC
- F:\Allfiles\Labs\10\Solution\MessageProcessor
- Update the storageConnectionString string constant by setting its value to the Connection string of the Storage account

### Test message queue access
- dotnet run
- Observe the output from the currently running console application. The output indicates that no messages are in the queue.
- Kill Terminal
- Open asyncstor[yourname]
- Create/Open queue  - messagequeue
- Add message -  Hello World
- dotnet run

### Delete queued messages
- Inspect the code to delete mesages in source code
- dotnet run
- Open queue  - messagequeue
- Its empty now

## Queue new messages by using .NET
## Write code to create queue messages
- Observe code to create new messages
- dotnet run

## View queued messages by using Storage Explorer
- Open queue  - messagequeue


# Lab 11: Monitoring services that are deployed to Azure
## Create and configure Azure resources
### Create an Application Insights resource
- Create resource - Insights
- Resource Group - MonitoredAssets
- Name: instrm[yourname]
- Create
- Copy  Instrumentation Key

### Create a web app by using Azure App Services resource
- Name: smpapi***[yourname]
- .NET Core 3.1 (LTS)
- Windows
- Enable Enable Application Insights in Monitoring tab
- Select the instrm[yourname] Application Insights
- Review + Create.
- Open Configuration
- Application settings tab.
- Find APPINSIGHTS_INSTRUMENTATIONKEY
  - Its set automatically when you built your Web Apps resource
- From the App Service blade, in the Settings category, select the Properties link.
- Record the value of the URL text box

### Configure web app autoscale options
- Scale out (App Service Plan)
- Custom autoscale
- In the Autoscale setting name text box, enter ComputeScaler.
- Scale based on a metric.
- 2, 8, 3
- Add a rule.
- Select Add
- Select Save

## Monitor a local web application by using Application Insights
### Build a .NET Web API project
- Open Visual Studio Code
- F:\Allfiles\Labs\11\Solution\Api
- dotnet run
- Open - http://localhost:5000/weatherforecast
- Close the currently running Visual Studio Code application.

### Get metrics in Application Insights
- Open  instrm[yourname]
- From the Application Insights blade, in the tiles in the center of the blade, find the displayed metrics. Specifically, find the number of server requests that have occurred and the average server response time.

#### Review
- You created an API by using ASP.NET and configured it to stream application metrics to Application Insights. You then used the Application Insights dashboard to get performance details

## Monitor a web app using Application Insights
### Deploy an application to the web app
- Open Visual Studio Code
- Open F:\Allfiles\Labs\11\Solution\Api
- az login
- az webapp list --resource-group MonitoredAssets --query "[?starts_with(name, 'smpapi')].{Name:name}" --output tsv
- cd F:\Allfiles\Labs\11\Solution
- Enter the following command, and then select Enter to deploy the api.zip file to the web app that you created earlier in this lab
    - az webapp deployment source config-zip --resource-group MonitoredAssets --src api.zip --name <name-of-your-api-app>
- Close the currently running Visual Studio Code application.
- Open smpapi***[yourname]
- Browse web app
  - Sample URL: https://smpapiatin.azurewebsites.net/weatherforecast

### Configure in-depth metric collection for Web Apps
- Open smpapi***[yourname]
- From the App Service blade, select Application Insights.
    - Ensure that the Application Insights section is set to Enable.
    - In the Instrument your application section, select the .NET tab.
    - In the Collection level section, select Recommended.
    - In the Profiler secton, select On.
    - In the Snapshot debugger section, select Off.
    - In the SQL Commands section, select Off.
    - Select Apply.
    - In the confirmation dialog, select Yes
- Open Web APP URL
  - Example: https://smpapiatin.azurewebsites.net/weatherforecast
- Observe the JSON array that’s returned as a result of using the API

### Get updated metrics in Application Insights
- Open instrm[yourname]
- From the Application Insights blade, in the tiles in the center of the blade, find the displayed metrics. Specifically, find the number of server requests that have occurred and the average server response time.

### View real-time metrics in Application Insights
- Open instrm[yourname]
- From the Application Insights blade, select Live Metrics Stream in the Investigate section.
- Open Web APP URL
  - Example: https://smpapiatin.azurewebsites.net/weatherforecast
- Observe the JSON array that’s returned as a result of using the API
- Return to your currently open browser window that’s displaying the Azure portal.
- Observe the updated Live Metrics Stream blade.

#### Review:
- Deployed your web application to Azure App Service and monitored your metrics from the same Application Insights instance.


# Lab 12: Enhancing a web application by using the Azure Content Delivery Network
## Create Azure resources
### Create a Storage account
- Name: contenthost[yourname]

### Create web App
- Name: landingpage[yourname]
- Docker Container
- Linux
- microsoftlearning/edx-html-landing-page:latest
- Copy URL of web app

## Configure Content Delivery Network and endpoints
### Open Azure Cloud Shell
- az provider register --namespace Microsoft.CDN

### Create a Content Delivery Network profile
- Create resource - CDN
- Name: contentdeliverynetwork
- Standard Akamai

### Configure Storage containers
- Open contenthost[yourname]
- + Container - media
- Blob (anonymous read access for blobs only)
- + Container
- video
- Blob (anonymous read access for blobs only)

### Create Content Delivery Network endpoints
- Open contentdeliverynetwork
- + Endpoint
- Name: cdnmedia[yourname]
- In the Origin type drop-down list, select Storage.
- In the Origin hostname drop-down list, select the contenthost[yourname].blob.core.windows.net option for the Storage account that you created earlier in this lab.
- In the Origin path text box, enter /media.
- In the Optimized for drop-down list, select General web delivery.
- Select Add

- Back on the CDN profile blade, select + Endpoint again.
    - In the Name text box, enter cdnvideo[yourname].
    - In the Origin type drop-down list, select Storage.
    - In the Origin hostname drop-down list, select the contenthost[yourname].blob.core.windows.net option for the Storage account that you created earlier in this lab.
    - In the Origin path text box, enter /video.
    - In the Optimized for drop-down list, select Video on demand media streaming.
    - Select Add

- Back on the CDN profile blade, select + Endpoint again
    - In the Name text box, enter cdnweb[yourname].
    - In the Origin type drop-down list, select Web App.
    - In the Origin hostname drop-down list, select the landingpage[yourname].azurewebsites.net option for the Web App that you created earlier in this lab.
    - In the Optimized for drop-down list, select General web delivery.
    - Select Add
#### Review
  - Registered the resource provider for Content Delivery Network and then used the provider to create both CDN profile and endpoint resources.

## Upload and configure static web content
### Observe the landing page
- Open landingpage[yourname] web app
- Browse
- Observe the error message displayed on the screen. The website won’t work until you configure the specified settings to reference multimedia content.

### Upload Storage blobs
- Open contenthost[yourname] storage account
- Open media container
- Upload
    - F:\Allfiles\Labs\12\Starter
      - campus.jpg
      - conference.jpg
      - poster.jpg
- Copy URL of this container from properties
- Open the video container
- Upload from F:\Allfiles\Labs\12\Starter\welcome.mp4
- Copy URL of this container from properties

### Configure Web App settings
- Open landingpage[yourname]
- On the App Service blade, in the Settings category, select the Configuration link.
- In the Configuration section, perform the following actions
    - Select the Application settings tab, and then select New application setting.
    - In the Add/Edit application setting pop-up window, in the Name text box, enter CDNMediaEndpoint.
    - In the Value text box, enter the URI value of the media container in the contenthost[yourname] storage account
    - Select OK to close the pop-up window.
    - Return to the Configuration section, and then select New application setting.
    - In the Add/Edit application setting pop-up window, in the Name text box, enter CDNVideoEndpoint.
    - In the Value text box, enter the URI value of the video container in the contenthost[yourname] storage account
    - Select OK to close the pop-up window
    - Return to the Configuration section, and then select Save
### Validate the corrected landing page
Open landingpage[yourname] web app
On the App Service blade, select Restart
Browse
Observe the updated website rendering multimedia content of various types
#### Review
- Uploaded multimedia content as blobs to Storage containers and then updated your Web App to point directly to the storage blobs.

## Use Content Delivery Network endpoints
### Retrieve endpoint URIs
- Open contentdeliverynetwork CDN profile
- Select the cdnmedia[yourname] endpoint
- Copy the value of the Endpoint hostname text box
- Close the Endpoint blade.
- Select the cdnvideo[yourname] endpoint
- On the Endpoint blade, copy the value of the Endpoint hostname text box
- Close the Endpoint blade

### Test multimedia content
- Construct a URL for the campus.jpg resource by combining the Endpoint hostname URL from the cdnmedia[yourname] endpoint that you copied earlier in the lab with a relative path of /campus.jpg
  - Note: For example, if your Endpoint hostname URL is https://cdnmediastudent.azureedge.net/, your newly constructed URL would be https://cdnmediastudent.azureedge.net/campus.jpg
- Construct a URL for the conference.jpg resource by combining the Endpoint hostname URL from the cdnmedia[yourname] endpoint that you copied earlier in the lab with a relative path of /conference.jpg.
  - Note: For example, if your Endpoint hostname URL is https://cdnmediastudent.azureedge.net/, your newly constructed URL would be https://cdnmediastudent.azureedge.net/conference.jpg

- Construct a URL for the poster.jpg resource by combining the Endpoint hostname URL from the cdnmedia[yourname] endpoint that you copied earlier in the lab with a relative path of /poster.jpg.
  - Note: For example, if your Endpoint hostname URL is https://cdnmediastudent.azureedge.net/, your newly constructed URL would be https://cdnmediastudent.azureedge.net/poster.jpg

- Construct a URL for the welcome.mp4 resource by combining the Endpoint hostname URL from the cdnvideo[yourname] endpoint that you copied earlier in the lab with a relative path of /welcome.mp4
  - Note: For example, if your Endpoint hostname URL is https://cdnvideostudent.azureedge.net/, your newly constructed URL would be https://cdnvideostudent.azureedge.net/welcome.mp4
- Open all theses URLs one by one in the browser in different tabs
  - Note: If the content isn’t available yet, the CDN endpoint is still initializing. This initialization process can take anywhere from 5 to 15 minutes.

## Update the Web App settings
- Open landingpage[yourname] web app
- On the App Service blade, in the Settings category, select the Configuration link.
- In the Configuration section, perform the following actions:
  - Select the Application settings tab
  - Select the existing CDNMediaEndpoint application setting
  - In the Add/Edit application setting pop-up dialog box, update the Value text box by entering the Endpoint hostname URL from the cdnmedia[yourname] endpoint that you copied earlier in the lab, and then select OK
  - Select the existing CDNVideoEndpoint application setting
  In the Add/Edit application setting pop-up dialog box, update the Value text box by entering the Endpoint hostname URL from the cdnvideo[yourname] endpoint that you copied earlier in the lab, and then selec OK
  - Select Save

    - Note: Wait for your application settings to persist before you move forward with the lab.

- Back in the Configuration section, select Overview.
- In the Overview section, select Restart

### Test the web content
- Open contentdeliverynetwork CDN profile
- Select the cdnweb[yourname] endpoint
- On the Endpoint blade, copy the value of the Endpoint hostname text box.
- In Browser, Open the Endpoint hostname URL for the cdnweb[yourname] endpoint
- Observe the website and multimedia content that are all served using Content Delivery Network.
