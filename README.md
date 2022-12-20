# Introduction 
Functions used to get data from Azure SQL

# Getting Started
Ensure you have a file called local.settings.json within the NptExplorer.AzureFunctions project (this is ignored by git)

This file holds local values used when you are running the functions locally

Set the file contents to 

{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "ConnectionString": "Data Source={{server}};Initial Catalog={{db}};Integrated Security=True"
  }
}

Update "LOCAL CONNECTION STRING" to the connection string for a local database.

Press F5. The displayed window will show the functions available - copy the URL of the function you wish to test and paste to a browser


# Build and Test
TODO: Describe and show how to build your code and run the tests. 
