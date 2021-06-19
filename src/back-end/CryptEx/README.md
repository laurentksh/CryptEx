# CryptEx-Backend

This is the back-end server, made in ASP.NET Core using Entity Framework Core.

## Technologies
Technologies we used and why:

### EF Core
I picked EF Core as I have quite some experience using it, and also because it's very easy to use.

### SignalR
We do use SignalR, mainly on the deposit-withdraw page.
I wanted the deposits' statuses to refresh automatically as Stripe was processing the payments.

I also never used it and really wanted to try it out !

### Coinbase
This is the package that provides access to the Coinbase API.
We use it to get exchange rates between cryptocurrencies and fiats. 
Though exchange rates between fiats are hardcoded.

### Testing: Moq
I picked Moq for tests because we use it at work and I thought it would help when dealing with dependency injection.


## Running locally

To run the project locally, please go to the CryptExApi folder, you will have to run several commands from here.

First, you will have to apply the migrations to the local MS-SQL database, by running this command: `dotnet ef database update`.

Please note that running the project on any other OS than Windows will not work. If you absolutely want to you can look into using an InMemory Database. (The NuGet package for it is already installed)

Finally, run `dotnet run --launch-profile CryptExApi`.

If everything is fine you should see `Now listening on: https://localhost:5001`

After that, when openning https://localhost:5001/swagger/ you should see all the endpoints listed.

## Tests

To run the unit tests, open a command prompt in the current folder and enter `dotnet test`.
This command will execute the tests and display the results.

## Common issues
Common issues you might encounter, and how to resolve them.

### Untrusted certificate
If the website doesn't work at all (i.e: all endpoints return errors), that might imply that your browser is blocking HTTP requests to the back-end server because the dev certificate hasn't been installed/isn't installed properly.

To resolve this issue, either try this command: `dotnet dev-certs https --trust` then re-run the server.

Or manually open an endpoint on your browser (by typing/copying the url manually, example: https://localhost:5001/api/Auth),
a prompt should appear saying that the website/website's certificate is untrusted.

In that case, click "Advanced" or "More" (depending on your browser), and click "Disable security warnings" (again this depends on your browser.).
After that the website should work.

### Any other issue
You can go to [this URL](https://stackoverflow.com/search) and type the error you see in the textbox at the top. ;)

## Author
Laurent Keusch [<@laurentksh>](https://github.com/laurentksh) - 2021