# models from https://github.com/ipublia/sentiment-analysis

import os
import requests
import json
import pickle
from keras.preprocessing.sequence import pad_sequences
from keras.models import load_model
import numpy as np
from atanet.sentiment.language.language import Language


DATA_DIR = os.path.join('/var/data/ipublia')
REMOTE_DATA_URL = 'https://www.ipublia.com/data/sentiment-analysis'
MAX_TEXT_LENGTH = 400

models = {}
tokenizers = {}

has_registered = False

language_map = {
    Language.French: 'fr',
    Language.English: 'en',
    Language.German: 'de',
    Language.Italian: 'it'
}


def load_remote_file(source_url, target_file):
    if not os.path.isdir(DATA_DIR):
        print('Creating data directory: ' + DATA_DIR)
        os.makedirs(DATA_DIR)

    if not os.path.isfile(target_file):
        print('Downloading {0} to {1}'.format(source_url, target_file))
        r = requests.get(source_url, timeout=10)
        if r.status_code == 200:
            data = r.content
            with open(target_file, 'wb') as f:                
                f.write(data)
            return 1
        else:
            print('Error ({0}) loading from {1}'.format(r.status_code, source_url))
            return -1
    return 0


def register():
    global lang_registry

    lang_registry = {}
    model_ids = ['en_1.0.1', 'de_1.0.1', 'fr_1.0.1', 'it_1.0.1']

    for model_id in model_ids:
        lang = model_id.split('_')[0]

        model_name = 'model_' + model_id + '.h5'
        model_file = os.path.join(DATA_DIR, model_name)
        model_url = REMOTE_DATA_URL + '/' + model_name

        # Load an register model
        load_remote_file(model_url, model_file)
        print('Loading model {0}'.format(model_file))
        model = load_model(model_file)
        model._make_predict_function()

        # Load and register tokenizer
        tokenizer_name = 'tokenizer_' + model_id + '.pickle'
        tokenizer_file = os.path.join(DATA_DIR, tokenizer_name)
        tokenizer_url = REMOTE_DATA_URL + '/' + tokenizer_name

        load_remote_file(tokenizer_url, tokenizer_file)
        print('Loading tokenizer {0}'.format(tokenizer_file))
        with open(tokenizer_file, 'rb') as handle:
            tokenizer = pickle.load(handle)
        lang_registry[lang] = { 'model': model, 'tokenizer': tokenizer }


def prepare_texts(tokenizer, texts):
    sequences = tokenizer.texts_to_sequences(texts)
    padded_texts = pad_sequences(sequences, maxlen=MAX_TEXT_LENGTH)
    return np.array(padded_texts)


def pretrained_predict(text: str, language: Language):
    global has_registered
    global language_map
    if not has_registered:
        register()
        has_registered = True

    lang = language_map[language]
    model = lang_registry[lang]['model']
    tokenizer = lang_registry[lang]['tokenizer']
    prepared_texts = prepare_texts(tokenizer, [text])
    predictions = model.predict(prepared_texts)
    return float(predictions[0][0])
