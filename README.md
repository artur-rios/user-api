# User Api

API for user management, authentication and authorization

Written in C#, using .Net 8

## Docs

- [System actors](./docs/actors.md)
- [System entities](./docs/entities.md)
- [Use cases](./docs/use-cases.md)
- [Database](./docs/database.md)

## Folder structure

📦database  
📦docs  
📦src  
 ┣ 📂TechCraftsmen.User.Api  
 ┃ ┣ 📂Controllers  
 ┃ ┣ 📂Properties  
 ┣ 📂TechCraftsmen.User.Configuration  
 ┃ ┣ 📂Authorization  
 ┃ ┣ 📂DependencyInjection  
 ┃ ┣ 📂Middleware  
 ┣ 📂TechCraftsmen.User.Core  
 ┃ ┣ 📂Collections  
 ┃ ┣ 📂Configuration  
 ┃ ┣ 📂Dto  
 ┃ ┣ 📂Entities  
 ┃ ┣ 📂Enums  
 ┃ ┣ 📂Exceptions  
 ┃ ┣ 📂Extensions  
 ┃ ┣ 📂Filters  
 ┃ ┣ 📂Interfaces  
 ┃ ┃ ┣ 📂Repositories  
 ┃ ┃ ┗ 📂Services  
 ┃ ┣ 📂Mapping  
 ┃ ┣ 📂Utils  
 ┃ ┣ 📂Validation  
 ┃ ┃ ┣ 📂Fluent  
 ┣ 📂TechCraftsmen.User.Data  
 ┃ ┣ 📂Relational  
 ┃ ┃ ┣ 📂Configuration  
 ┃ ┃ ┣ 📂Mapping  
 ┃ ┃ ┗ 📂Repositories  
 ┗ 📂TechCraftsmen.User.Services  
 📦tests  
 ┣ 📂TechCraftsmen.User.Api.Tests  
 ┣ 📂TechCraftsmen.User.Core.Tests  
 ┃ ┣ 📂Entities  
 ┃ ┣ 📂Utils  
 ┃ ┣ 📂Validation  
 ┃ ┃ ┣ 📂Fluent  
 ┣ 📂TechCraftsmen.User.Services.Tests  
 ┗ 📂TechCraftsmen.User.Tests.Utils  
 ┃ ┣ 📂Attributes  
 ┃ ┣ 📂Functional  
 ┃ ┣ 📂Generators  
 ┃ ┣ 📂Mock  
