# Documentation
* [C4](https://structurizr.com/share/78815/diagrams#)
* [UML](./doc/UML.pdf)
# Build instructions
1. Ensure you have Docker installed (https://www.docker.com/products/docker-desktop/).
2. Clone the project and navigate to its directory
```
git clone https://git.fhict.nl/I491971/s3-chat-bot.git --depth=1
cd ./s3-chat-bot/
```
3. Open a terminal and execute:
```
docker compose up
```
- Alternatively, you can run the `ai` profile, assuming you have a RTX 3080:
```
docker compose --profile=ai up
```
3. Visit [http://localhost:3000](http://localhost:3000) in your browser.
4. [WIP] Fill in the following credentials:
> Tech support
- Email: `techsupport@gmail.com`
- Password: `password`
> Customer
- Email: `customer@gmail.com`
- Password: `password`
## Connecting with the database
You can use either [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16) or the dockerized [Adminer](https://en.wikipedia.org/wiki/Adminer) instance with URL [http://localhost:8080](http://localhost:8080).
### Credentials
- Host: `172.23.0.2:1433`
- Username: `SA`
- Password: `Password1234`
- Schema: `BasWorld`