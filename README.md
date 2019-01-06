# Atanet
Atanet is a web application, where everybody can freely write posts and vote for posts others have already written. Those posts can be sorted with a variety of different filters. What makes Atanet special is, that every post will be given a class classified by a RNN implemented with TensorFlow.
The technologies used in this project are ASP.NET Core, Flask, TensorFlow and Angular.

# Running locally
`docker network create atanet-network`

`docker volume create atanet-db-volume`

`docker volume create atanet-py-volume`

Generate a client id in your Google Cloud Console.

Add `.env` file and replace `client_id` with generated Google Client ID.
Also update the `clientId` in `app/src/assets/Development.config.json`, `app/src/assets/Production.config.json` and `app/src/assets/.config.json`.

```
ASPNETCORE_ENVIRONMENT=Development
GOOGLE_CLIENT_ID=client_id
SCOREBOARD_ENTRIES=5

MYSQL_ROOT_PASSWORD=password
MYSQL_USER=atanet
MYSQL_PASSWORD=password
MYSQL_DATABASE=Atanet
DATABASE_PORT=3306
# either db or ip
DATABASE_HOST=db
SENTIMENT_HOST=py
SENTIMENT_PORT=9003
ASPNETCORE_URLS=http://localhost:9000
```

Create certifictes for nginx reverse proxy:
`openssl req -x509 -newkey rsa:4096 -nodes -out ./nginx/config/cert.pem -keyout ./nginx/config/key.pem -days 365`

`docker-compose up --build`

# Connecting to database:
`mysql -u atanet -h 172.19.0.1 -P 9001 -D Atanet -p`

# Train TensorFlow model with CPU
- `cd py && pipenv shell`
- Run `python -m pip uninstall --yes tensorflow-gpu && python -m pip install tensorflow`
- Revert with `python -m pip uninstall --yes tensorflow && python -m pip install tensorflow-gpu`
