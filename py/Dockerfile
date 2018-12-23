FROM python:3.6.5-stretch
WORKDIR /serve
COPY ./ ./

ENV PYTHONPATH /serve

RUN rm -f Pipfile.lock && \
    pip install pipenv && \
    pipenv lock -r > requirements.txt && \
    pip install -r requirements.txt && \
    pip uninstall --yes tensorflow-gpu && \
    pip install tensorflow
