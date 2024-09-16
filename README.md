# remote-pokedex

In order to start the application on your local machine you have to:
1. Download the correct docker version for your machine from: https://www.docker.com/products/docker-desktop/.
1. Install docker on your local machine using the installer you have just downloaded.
1. Clone the repository to a local folder using git using the URL https://github.com/mbarbaro09/remote-pokedex.git.
1. Open a terminal (cmd).
1. Change directory to ../remote-pokedex (or ..\remote-pokedex)
1. run > docker build -t remote-pokedex-image -f Dockerfile .
1. run > docker run -p 5000:8080 -it --rm remote-pokedex-image
1. Now the docker container will be running and you can call the application via the endpoint http://localhost:5000/pokemon