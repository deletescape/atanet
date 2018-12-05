# Atanet
Atanet is a web application, where everybody can freely write posts and vote for posts others have already written. Those posts can be sorted with a variety of different filters. What makes Atanet special is, that every post will be given a class classified by a RNN implemented with TensorFlow.
The technologies used in this project are ASP.NET Core, Flask, TensorFlow and Angular.

`docker network create atanet-network`

`docker volume create atanet-volume`

`cd app && npm i && cd ..`

Add `.env` file

`
ASPNETCORE_ENVIRONMENT=Development
GOOGLE_CLIENT_ID=
GOOGLE_CLIENT_SECRET=

MYSQL_ROOT_PASSWORD=password
MYSQL_USER=atanet
MYSQL_PASSWORD=password
MYSQL_DATABASE=Atanet
DATABASE_PORT=3306

HOST_ENVIRONMENT=Development
`



`docker-compose up`

Connect to db:
`mysql -u atanet -h 172.19.0.1 -P 9001 -D Atanet -p`