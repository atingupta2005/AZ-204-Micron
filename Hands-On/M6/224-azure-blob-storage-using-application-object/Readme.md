- Create storage account and container and upload a file to it
	- sajune21
	- container1
		- Private access
	- file name: sample.txt
	- file URL: https://sajune21.blob.core.windows.net/container1/sample.txt
- Create application registration in Azure AD
	- appblob
	- client id: fe940b0e-332f-431b-9c22-2028a410a3ea
	- tenant id: 6bb2f9af-a0af-4c32-a5ec-5f7011d37551
	- Secret: l9eDJWIy~1.MFHnuYw..MM01RHY3ma81O.
	
- Give rights to app object in Storage account RBAC
	- Reader
	- StorageBlobDataReader
- Change details in code - 224-azure-blob-storage-using-application-object
	- 