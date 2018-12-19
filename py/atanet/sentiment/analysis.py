import tensorflow as tf
from tensorflow import keras
import numpy as np
from atanet.sentiment.language.language import Language
from atanet.sentiment.datasets.english import get_english_data
from atanet.sentiment.datasets.german import get_german_data
from atanet.sentiment.datasets.simplified_chinese import get_simplified_chinese_data


def train_model(language: Language) -> keras.Model:
    (x_train, y_train), (x_test, y_test), max_features, max_length = get_dataset(language)
    model = get_model(max_features, max_length)
    model.compile(loss='binary_crossentropy', optimizer='adam', metrics=['acc'])
    model.fit(x=[x_train], y=[y_train], epochs=1)
    loss, accuracy = model.evaluate(x=[x_test], y=[y_test])
    print('Accuracy: %f' % (accuracy * 100))


def get_model(max_features, max_length) -> keras.Model:
    # try something like: https://stackoverflow.com/questions/51363709/how-to-predict-sentiment-analysis-using-keras-imdb-dataset
    model = keras.Sequential()
    model.add(keras.layers.Embedding(max_features, 64, input_length=max_length))
    model.add(keras.layers.LSTM(128, dropout=0.2, recurrent_dropout=0.2))
    model.add(keras.layers.Dense(64, activation='relu'))
    model.add(keras.layers.Dense(1, activation='sigmoid'))
    return model


def get_dataset(language: Language):
    if language == Language.German:
        return get_german_data()
    elif language == Language.English:
        return get_english_data()
    elif language == Language.SimplifiedChinese:
        return get_simplified_chinese_data()
    else:
        return None
