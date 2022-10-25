#FirstNetWebPrintOnline API

This project is the back-end RESTful API portion of my FirstNet WebPrintOnline web app written in ASP.NET Core, C# with SQL portion utilizing Entity FrameWork. 

It does include implementation of basic authentication and authorization which is managed by Auth0.com. 

The web app, upon the user entering a six digit order number and clicking submit will call the REST API POST method. Loading the page will call the REST API GET method 
and update the table where the data is stored in an SQL database. The database stores the last 5 requests.

The bulk of the actual work is done on a server-side C# .NET Framework console application. The workflow process (label printing) that was done manually is automated through the server-side application. The fron-end, back-end REST API and database simply serve as a means of communicating the desired order to automate. This allows
multiple people to submit print requests.
