import tensorflow as tf
from tensorflow import keras
import pickle
import json
from atanet.sentiment.language.language import Language, language_to_string
from atanet.sentiment.datasets.set import SentimentDataset


class LanguageModel:

    def __init__(self, dataset: SentimentDataset):
        self.epochs = 1
        if dataset is not None:
            number_of_words = dataset.get_word_count()
            input_length = dataset.get_max_x_text_length()
            self._init_model(input_length, number_of_words)
            self._name = language_to_string(dataset.get_language())
            self._dataset = dataset
            self._tokenizer = self._dataset.get_tokenizer()
            self._max_x_length = self._dataset.get_max_x_text_length()


    @staticmethod
    def create_loaded(language: Language):
        instance = LanguageModel(None)
        instance._name = language_to_string(language)
        instance.load()
        return instance


    def train(self):
        if self._dataset is None:
            raise ValueError('Cannot be trained without dataset')
        (x_train, y_train), (x_test, y_test) = self._dataset.get_data()
        self._max_x_length = self._dataset.get_max_x_text_length()
        self._model.fit(x_train, y_train, validation_data=(x_test, y_test), epochs=self.epochs, batch_size=64)
        scores = self._model.evaluate(x_test, y_test, verbose=0)
        return scores[1]


    def print(self):
        self._model.summary()


    def save(self):
        self._model.save(self.get_model_name())
        with open(self.get_tokenizer_name(), 'wb') as f:
            pickle.dump(self._tokenizer, f, protocol=pickle.HIGHEST_PROTOCOL)
        with open(self.get_metadata_name(), 'w') as f:
            json.dump({
                'max_x_length': self._max_x_length
            }, f)


    def load(self):
        self._model = keras.models.load_model(self.get_model_name())
        with open(self.get_tokenizer_name(), 'rb') as f:
            self._tokenizer = pickle.load(f)
        with open(self.get_metadata_name(), 'r') as f:
            loaded_json = json.load(f)
            self._max_x_length = loaded_json['max_x_length']


    def predict(self, x):
        sequences = self._tokenizer.texts_to_sequences(x)
        to_predict = keras.preprocessing.sequence.pad_sequences(sequences, maxlen=self._max_x_length, padding='post')
        return self._model.predict(to_predict)


    def get_model_name(self):
        return f'./trained_{self._name}.hdf5'


    def get_tokenizer_name(self):
        return f'./trained_{self._name}_tokenizer.pickle'


    def get_metadata_name(self):
        return f'./trained_{self._name}_metadata.json'


    def _init_model(self, input_length: int, number_of_words: int):
        model = keras.Sequential()
        model.add(keras.layers.Embedding(input_dim=number_of_words, output_dim=64, input_length=input_length))
        model.add(keras.layers.SpatialDropout1D(0.3))
        model.add(keras.layers.Conv1D(activation='relu', padding='same', filters=64, kernel_size=5))
        model.add(keras.layers.MaxPooling1D(pool_size=4))
        model.add(keras.layers.LSTM(250, dropout=0.2, recurrent_dropout=0.2))
        model.add(keras.layers.Dense(1, activation='sigmoid'))
        model.compile(loss='binary_crossentropy', optimizer='adam', metrics=['accuracy'])
        self._model = model
