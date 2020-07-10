
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
  - [Setup Procedure](#setup-procedure)
    - [Local Environment](#local-environment)
    - [Docker Environment](#docker-environment)
    - [Configuration Options](#configuration-options)
      - [Custom App configuration - appsettings.json](#custom-app-configuration---appsettingsjson)
  - [Testing](#testing)
    - [Local Test Environment](#local-test-environment)
    - [Docker Test Environment](#docker-test-environment)
    - [Travis CI Environment](#travis-ci-environment)
- [Usage](#usage)
- [Design Choices](#design-choices)
  - [Application Structure](#application-structure)
  - [Configuration Options](#configuration-options-1)
  - [Happiness decay and Hungriness Increase](#happiness-decay-and-hungriness-increase)
- [Contact](#contact)



<!-- ABOUT THE PROJECT -->
## About The Project

This small project has been developed as part of a Technical Test for a recruiting process. The resulting development is an HTTP API for a hypothetical Pet Game that should fulfill the following loose requirements:

 - Users have animals 
 - Stroking animals makes them happy 
 - Feeding animals makes them less hungry 
 - Animals start “neutral” on both metrics
 - Happiness decreases over time/hunger increases over time (even when the user is offline) 
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

There are several options and tools to set up the environment, but having either an environment with [.NET Core 3.1](https://dotnet.microsoft.com/) or a  [Docker](https://docker.com) daemon running and *Docker-Compose* will allow executing both the API and its Test Suite.

Clone the repository with:

    git clone https://github.com/Mandros7/mediatonic-pets-test.git

### Setup Procedure

#### Local Environment

In a local .NET environment, it is expected that the command `dotnet` is reachable on the execution PATH. Run the following commands to prepare the solution and start execution:

    cd <PROJECT_ROOT_PATH>
    dotnet restore
    dotnet build
    dotnet run
    
This will start the service on `http://localhost:5000` by default and connect to a cloud mLab MongoDB deployment made available for demonstration purposes. Navigating to `http://localhost:5000` will open a Swagger Web Interface allowing direct interaction with the API.

To use a different MongoDB deployment, it is possible to specify a different Database Endpoint by declaring the following environment variable:

*Windows - PowerShell*

    $env:MONGODB_HOST="<mongodb://mongo-path-with-credentials>"

*Linux/Unix - Bash*

    export MONGODB_HOST="<mongodb://mongo-path-with-credentials>"

#### Docker Environment
Assuming there is an already running Docker daemon, and *docker-compose* is available in your execution PATH, it is possible to build and run the Web API with no need to install MongoDB or rely on cloud deployments by simply executing:

    cd <PROJECT_ROOT_PATH>
    docker-compose -f docker-compose.yml build
    docker-compose -f docker-compose.yml up -d

This will leave the Web API running on the background with Docker's assigned IP and port 80 by default. Therefore, navigating to `http://localhost:80` opens up the API Interface allowing us to interact directly with the API. To stop the application, execute:

    docker-compose -f docker-compose.yml down

#### Configuration Options

Apart from the `MONGODB_HOST` variable that allows us to point the API towards any MongoDB back-end, the application allows us to override the default values for Pet objects generated during execution. This allows testing a different combination of values without having to interact directly with the code. 

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
**NOTE:** The current implementation will only load values during the initialization of the Web API.

### Testing

Similar to the previous section containing a setup procedure, both development and Docker-based environments are supported. 

#### Local Test Environment 

Assuming command `dotnet` is reachable on the execution PATH, run the following commands to prepare the solution, and start execution of tests:

    cd <PROJECT_ROOT_PATH>
    dotnet restore
    dotnet test
    
#### Docker Test Environment
Assuming there is an already running Docker daemon, and *docker-compose* is available in your execution PATH, it is possible to build and run the Web API with no need to install MongoDB or rely on cloud deployments by simply executing:

    cd <PROJECT_PATH>
    docker-compose -f docker-compose-test.yml build
    docker-compose -f docker-compose-test.yml up -d
    
This will leave the environment ready for tests. Use the following command to execute them:

    docker exec -it mediatonic-pets-test_pet-api-test_1 -- dotnet test
Once the testing environment is no longer needed, execute the following command to destroy it:

    docker-compose -f docker-compose.tests.yml down

#### Travis CI Environment

In addition to the methods mentioned above, a simple Travis - Github integration has been put in place, forcing test execution to take place after each commit detected by Travis and leaving a trace in the [Build Dashboard](https://travis-ci.com/github/Mandros7/mediatonic-pets-test).



<!-- GETTING STARTED -->
## Usage
The Swagger endpoint reachable through the root path of the Web Service provides the means to test the application in a self-explanatory manner.

## Design Choices
A few technical design choices regarding this development should be highlighted:

### Application Structure

The application is split into 4 main folders:

 - Controllers: handling API requests 
 - Models: handling the data model for Pets and Users 
 - Services: bridging both Models and Controllers with the MongoDB back-end
 - Factories: acting as service dependencies that abstract the generation of new Pet objects

In particular, the usage of a Factory pattern was deemed necessary to allow the dynamic creation of different kinds of pets. 

The initial statement for this development does not state too many differences between pets (they share the same set of metrics), therefore there is no need for complex inheritance structure. In future extensions they could potentially be expanded with more metrics, some of them unique for specific pets, or even require the usage of inheritance creating Pets that expose different kinds of methods. 

The usage of a Factory pattern will benefit the design by allowing to extend the code base, instead of modifying it, while also allowing to insert configuration files on constructors to customize metrics and behavior for each Pet.

### Configuration Options
 The application performs parsing of configuration files to override default Pet values when necessary. This allows tweaking of values on testing and staging environments in a more iterative way, reducing the need to modify lines of code.

### Happiness decay and Hungriness Increase

*NOTE*: Metric rates in this case are set per minute.

While it is tempting to generate Asynchronous Tasks to handle the update of pets, the code in this solution allows us to update Pet metrics *only when it's necessary*, that is, *only when the End-User requests to see them*. 

From a business point of view, this enables a directly proportional relationship between the processing resources dedicated to serving API calls, and the activity of users. Idle end-users will only consume storage capacity on the database, which could be pruned based on operational necessities.

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
