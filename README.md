[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
![Build][build-shield]

<!-- PROJECT LOGO -->
<br />
<p align="center">
  <h1 align="center">Pets API</h1>

  <p align="center">
    Test Server Implementation
    <br />
  </p>
</p>



<!-- TABLE OF CONTENTS -->
## Table of Contents

- [Table of Contents](#table-of-contents)
- [About The Project](#about-the-project)
  - [Built With](#built-with)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Testing](#testing)
- [Usage](#usage)
- [Design Choices](#design)
- [Contact](#contact)



<!-- ABOUT THE PROJECT -->
## About The Project

This small project has been developed as part of a Technical Test for a recruiting process. The resulting development is an HTTP API for an hypothetical Pet Game that should fulfill the following loose requirements:

 - Users have animals 
 - Stroking animals makes them happy 
 - Feeding animals makes them less hungry 
 - Animals start “neutral” on both metrics
 - Happiness decreases over time / hunger increases over time (even when the user is offline) 
 - Users can own multiple animals of different types 
 - Different animal types have metrics which increase/decrease at different rates

### Built With

* [.NET Core 3.1](https://dotnet.microsoft.com/)
* [MongoDB](https://www.mongodb.com)
* [Visual Studio Code](https://code.visualstudio.com/)
* [Docker](https://docker.com)

<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple steps.

### Prerequisites

There are several options and tools to setup the environment, but having either an environment with [.NET Core 3.1](https://dotnet.microsoft.com/) or alternatively a  [Docker](https://docker.com) daemon running and *Docker-Compose* will allow to execute both the API and its Test Suite.

Clone the repository with:

    git clone https://github.com/Mandros7/mediatonic-pets-test.git

### Setup Procedure

#### Local .NET Environment

In a local .NET environment, it is expected that the command `dotnet` is reachable on the execution PATH. Run the following commands to prepare the solution and start execution:

    cd <PROJECT_ROOT_PATH>
    dotnet restore
    dotnet build
    dotnet run
    
This will start the service on `http://localhost:5000` by default and connect to a cloud mLab MongoDB deployment made available for demonstration purposes. Navigating to `http://localhost:5000` will open a Swagger Web Interface allowing direct interaction with the API.

In order to use a different mongoDB deployment, it is possible to specify a different Database Endpoint by declaring the following environment variable:

*Windows - PowerShell*

    $env:MONGODB_HOST="<mongodb://mongo-path-with-credentials>"

*Linux/Unix - Bash*

    export MONGODB_HOST="<mongodb://mongo-path-with-credentials>"

#### Docker Environment
Assuming there is an already running Docker daemon, and *docker-compose* is available in your execution PATH, it is possible to build and run the Web API with no need to install mongoDB or rely on cloud deployments by simply executing:

    cd <PROJECT_ROOT_PATH>
    docker-compose -f docker-compose.yml build
    docker-compose -f docker-compose.yml up -d

This will leave the Web API running on the background with docker's assigned IP and port 80 by default. Therefore, navigating to `http://localhost:80` opens up the API Interface allowing to interact directly with the API. In order to stop the application, execute:

    docker-compose -f docker-compose.yml down

#### Configuration Options

Apart from the `MONGODB_HOST` variable that allows to point the API towards any mongoDB back-end, the application allows to override the default values for Pet objects generated during execution. This allows to test different combination of values without having to interact directly with the code. 

##### Custom App configuration - appsettings.json

It is possible to edit and modify the values for pets using the appsettings.json file. The following snippet showcases an example of its contents:
```json
"GlobalPetConfigurationSettings": {
	"PetConfigurationSettings": [
		{
			"Type": "dog",
			"Happiness" : 50.0,
			"Hungriness" : 50.0,
			"HappinessRate": -0.3,
			"HungrinessRate": 0.4,
			"StrokeHappiness": 5.0,
			"FeedHungriness": -10.0
		},
		{
			"Type": "cat",
			"Happiness" : 50.0,
			"Hungriness" : 50.0,
			"HappinessRate": -0.9,
			"HungrinessRate": 0.2,
			"StrokeHappiness": 8.0,
			"FeedHungriness": -5.0
		}
	]
}
```
**NOTE:** The current implementation will only load values during initialization of the Web API.

### Testing

Similar to the previous section containing a setup procedure, both development a Docker-based environments are supported. 

#### Local .NET Environment

Assuming command `dotnet` is reachable on the execution PATH, run the following commands to prepare the solution and start execution of tests:

    cd <PROJECT_ROOT_PATH>
    dotnet restore
    dotnet test
    
#### Docker Environment
Assuming there is an already running Docker daemon, and *docker-compose* is available in your execution PATH, it is possible to build and run the Web API with no need to install mongoDB or rely on cloud deployments by simply executing:

    cd <PROJECT_PATH>
    docker-compose -f docker-compose.tests.yml build
    docker-compose -f docker-compose.tests.yml up -d
    
This will leave the environment ready for tests. Use the following command to execute them:

    docker exec -it mediatonic-pets-test_pet-api-test_1 -- dotnet test
Once the testing environment is no longer needed, execute the following command to destroy it:

    docker-compose -f docker-compose.tests.yml down

#### Travis CI Environment

In addition to the methods mentioned above, a simple Travis - Github integration has been put in place, forcing test execution to take place after each commit detected by Travis and leaving a trace in the [Build Dashboard](https://travis-ci.com/github/Mandros7/mediatonic-pets-test).

<!-- CONTACT -->
## Contact

Héctor Rodríguez - hectorrcov93@gmail.com

Project Link: [https://github.com/Mandros7/mediatonic-pets-test](https://github.com/Mandros7/mediatonic-pets-test)

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/Mandros7/mediatonic-pets-test.svg?style=flat-square
[contributors-url]: https://github.com/Mandros7/mediatonic-pets-test/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/Mandros7/mediatonic-pets-test.svg?style=flat-square
[forks-url]: https://github.com/Mandros7/mediatonic-pets-test/network/members
[stars-shield]: https://img.shields.io/github/stars/Mandros7/mediatonic-pets-test.svg?style=flat-square
[stars-url]: https://github.com/Mandros7/mediatonic-pets-test/stargazers
[issues-shield]: https://img.shields.io/github/issues/Mandros7/mediatonic-pets-test.svg?style=flat-square
[issues-url]: https://github.com/Mandros7/mediatonic-pets-test/issues
[build-shield]: https://www.travis-ci.com/Mandros7/mediatonic-pets-test.svg?branch=master
