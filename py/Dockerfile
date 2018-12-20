FROM python:3.6
WORKDIR /serve
RUN export PATH="$PATH:/root/.local/bin"
COPY ./ ./
RUN python -m pip install --user pipenv
RUN pipenv install
CMD ["python", "-m", "atanet", "serve"]
