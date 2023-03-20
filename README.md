<a name="readme-top"></a>

<br />

<div align="center">
<h1 >CryptoCop</h1>
</div>

<div  align="center">
<p><a href="/assignment-description.pdf" title="Project Assignment Pdf">
  <img src="images/projectPhoto.png" alt="Google logo" width="300" height="400">
</a></p>
<p>A backend for a cryptocurrency shop</p>
</div>
<br>

<!-- ABOUT THE PROJECT -->
## About The Project

This project consists of four microservices:

Web API written in .NET Core, which collects data regarding available cryptocurrencies and exchanges.

Payment service written in python that check for valid credit/debit card and sets up a payment queue via Rabbitmq.

Email service which is written in python, sends a confirmation email to the user via email queue from Rabbitmq.

RabbitMQ works as a glue between the other services to communicate with each other via AMQP.

Postgresql (elephantSql) is used to store, Users, hashedPasswords, addresses, JwtTokens, Orders, OrderItems, PaymentCards (censored) and ShoppingCartItems.

Then finally Docker is used to combine all these services into a single container.



### Built With

* [![Python][Python]][Python-url]
* [![Dotnet][Dotnet]][Dotnet-url]
* [![RabbitMQ][RabbitMQ]][RabbitMQ-url]
* [![Postgres][Postgres]][Postgres-url]
* [![Docker][Docker]][Docker-url]


<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple steps.

### Prerequisites

List of things you need to use the software and how to install them.

* Docker
  You will need to install docker to build the project into a single container.

* Cryptocop.Software.API
  You will need to add your own Sql host string inside appsettings.json
  ```
  "ConnectionStrings": {
    "CryptocopConnectionString": "SQLHostStringHERE"
  },
  ```
  If you go with elephantsql like i did, then your string will look like this:
  ```
  postgres://username:password@lucky.db.elephantsql.com/username
  ```
  and set you info into this string:
  ```
   "CryptocopConnectionString": "Host=lucky.db.elephantsql.com;Username=USERNAMEHERE;Password=PASSWORDHERE;Database=USERNAMEHERE"
  ```
  
* Cryptocop-emails
  If you want to be able to send an email, you will have to add your own service. I used mailgun, you will see the python comments referencing how it should look inside the code. 
  ```python
  email_address = "EMAILSERVICEGOESHERE"   #Example: https://api.mailgun.net/v3/sandboxa*********.mailgun.org/messages
  email_auth = "EMAILAUTHCODE"
  email_from = "Who it is from"  #CryptoCop <mailgun@sandboxa*********.mailgun.org>
  ```



## Build & Starting Container

To create the docker container open up a terminal in the root project folder where the docker-compose.yml file is.
Type:
  ```
  docker-compose up
  ```
And it should build the container.

Now you can use the docker command from below:
  ```
  docker-compose restart: restarts the services specified in the compose file.
  
  docker-compose stop: stops the services specified in the compose file.
  
  docker-compose start: starts the services specified in the compose file.
  ```
  
## Usage

  When the container is up and running you can check on swagger for the endpoints.
  ```
  http://localhost:<port>/swagger/index.html
  ```
  
  ```
  http://localhost:5000/swagger/index.html
  ```
  <img src="images/swaggerUI.png">

## Roadmap

  Create a frontend for a better showcase


<!-- CONTACT -->
## Contact

Elfar Snær Arnarson - elfarsnaer@gmail.com

Project Link: [https://github.com/PanamaP/CryptoCop](https://github.com/PanamaP/CryptoCop)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[Python]:https://img.shields.io/badge/Python-3776AB?style=for-the-badge&logo=python&logoColor=white
[Python-url]:https://www.python.org/
[Dotnet]:https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white
[Dotnet-url]:https://dotnet.microsoft.com/en-us/
[RabbitMQ]:https://img.shields.io/badge/rabbitmq-%23FF6600.svg?&style=for-the-badge&logo=rabbitmq&logoColor=white
[RabbitMQ-url]:https://www.rabbitmq.com/
[Postgres]:https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white
[Postgres-url]:https://www.postgresql.org/
[Docker]: https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white
[Docker-url]:https://www.docker.com/
