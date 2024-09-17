# remote-pokedex

### In order to start the application on your local machine you have to:
1. Download the correct docker version for your machine from: https://www.docker.com/products/docker-desktop/.
1. Install docker on your local machine using the installer you have just downloaded.
1. Clone the repository to a local folder using git using the URL https://github.com/mbarbaro09/remote-pokedex.git.
1. Start the docker engine (by opening the docker desktop app)
1. Open a terminal (cmd).
1. Change directory to ../remote-pokedex/src/remote-pokedex (or ..\remote-pokedex\src\remote-pokedex). Be sure to find the Dockerfile inside the directory
1. run > docker build -t remote-pokedex-image -f Dockerfile .
1. run > docker run -p 5000:8080 -it --rm remote-pokedex-image
1. Now the docker container will be running and you can call the application via the endpoint http://localhost:5000/pokemon

## Production changes
In case we wanted to deploy the application in production and open to the internet it would be wise to make some changes not implemented now for semplicity.

### Authentication & Authorization
The first thing to do would be to restrict access to the API by introducing an authentication system. One choice could be to implement the OAuth2.0 protocol and to use JWT tokens to send the signed user data to the server for authentication and authorization.

We would also need to add an authentication API for handling sign in of users and token distribution and refresh.

### API Rate limiting
Secondly a smart choice would be to add a mechanism of rate limiting to deter DOS attacks. That could be easily implemented by using cloud services like Azure App Gateway.

### Caching
Because the response generated for a particular request never changes we could add a caching layer: either using the server memory or, in case of responses with a big size, a dedicated external service like Redis cache.

### Scaling
Thanks to containerization we the opportunity to deploy multiple instances of the same sontainer to obtain horizontal scaling. A tool for this could be Kubernetes.