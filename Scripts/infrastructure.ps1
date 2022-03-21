# general variables
$resourceGroupName = "azeventhub"
$location = "westus3"

# Event Hub variables
$eventHubName = "eventhubkhododemo"

# Key Vault variables
$keyVaultName = "eventhubdemokeyvault"

# Storage Account variables
$storageAccountName = "eventhubdemostoragekh"
$storageAccountKind = "StorageV2"
$storageAccountSku = "Standard_RAGRS"
$storageAccountContainerName = "azeventgriddemo"

az login

WRITE-HOST "Logged in to Azure!!!"

# set to <your account id>
az account set --subscription "efdefaf7-e7a2-4706-8457-484103773250"

az group create --name $resourceGroupName --location $location

# Event Hub
az eventhubs namespace create --name $eventHubName --resource-group $resourceGroupName --location $location

az eventhubs eventhub create --name $eventHubName --resource-group $resourceGroupName --namespace-name $eventHubName

# Key Vault
az keyvault create --name $keyVaultName --resource-group $resourceGroupName --location $location

az keyvault secret set --vault-name $keyVaultName --name "EventHubListenCS" --value "<Create Key in Event Hub and Update This>"

az keyvault secret set --vault-name $keyVaultName --name "EventHubReadCS" --value "<Create Key in Event Hub and Update This>"

# Storage Account
az storage account create --name $storageAccountName --resource-group $resourceGroupName --location $location --sku $storageAccountSku --kind $storageAccountKind

az storage container create --name $storageAccountContainerName --account-name $storageAccountName --auth-mode login