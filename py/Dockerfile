FROM python:3.6.5-stretch
WORKDIR /serve
COPY ./ ./

RUN rm -f Pipfile.lock && \
    pip install pipenv && \
    pipenv install --deploy && \
    pipenv run python -m pip uninstall --yes tensorflow-gpu && \
    pipenv run python -m pip install tensorflow
CMD ["pipenv", "run", "python", "-m", "atanet", "serve"]
