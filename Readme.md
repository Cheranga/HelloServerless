# Notes

### Remote debugging

* Open `Cloud Explorer`.
* Browse to `AppServices` and locate the function app.
* Right-click and select `Attach to debugger` option. This will take some time.
* Add a debug point in the code.
* Send the request to the deployed function.
* Once you hit the debug point, do your thing!

### Azure Storage
* __LESSON LEARNED IN A HARD WAY! - YOU CANNOT STORE OBJECTS INSIDE A BLOB! (OF COURSE YOU ##@#%!, BUT NOW I KNOW!)__

* There are multiple ways to set an output binding
  * Define an `out` parameter.
  * If you want to use `async/await` capabilities inside your function (of course!), make the parameter of type `IAsyncCollector<T>`.
  * Decorate the method with the `return(typeof(T))` attribute with `T` being the return type of the method.

```CSharp
[FunctionName("MakeApplication")]
[return: Queue("loan-applications")]
        public static async Task<LoanApplication> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,            
            ILogger log)
        {
			//
			// When the function returns the loan application request object it will endup being added to the "loan-applications" queue.
			//
			return new LoanApplicationRequest();
		}

```

* In `local.settings.json` file these are the default contents.

```JSON
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "AzureWebJobsDashboard": "UseDevelopmentStorage=true"
    }
}
```

* Can I use a key instead of the value when using the `AzureWebJobsStorage`?