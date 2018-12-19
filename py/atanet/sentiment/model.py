import tensorflow as tf
from tensorflow import keras
import pickle
from atanet.sentiment.language.language import Language, language_to_string
from atanet.sentiment.datasets.set import SentimentDataset


class LanguageModel:

    def __init__(self, number_of_words: int, input_length: int, dataset: SentimentDataset):
        model = keras.Sequential()
        model.add(keras.layers.Embedding(input_dim=number_of_words, output_dim=64, input_length=input_length))
        model.add(keras.layers.SpatialDropout1D(0.3))
        model.add(keras.layers.Conv1D(activation='relu', padding='same', filters=64, kernel_size=5))
        model.add(keras.layers.MaxPooling1D(pool_size=4))
        model.add(keras.layers.LSTM(250, dropout=0.2, recurrent_dropout=0.2))
        model.add(keras.layers.Dense(1, activation='sigmoid'))
        model.compile(loss='binary_crossentropy', optimizer='adam', metrics=['accuracy'])
        self._model = model
        self._name = language_to_string(dataset.get_language())
        self._dataset = dataset
        self._tokenizer = self._dataset.get_tokenizer()


    def train(self):
        (x_train, y_train), (x_test, y_test) = self._dataset.get_data()
        self._model.fit(x_train, y_train, validation_data=(x_test, y_test), epochs=3, batch_size=64)
        scores = self._model.evaluate(x_test, y_test, verbose=0)
        return scores[1]


    def print(self):
        self._model.summary()


    def save(self):
        self._model.save(self.get_model_name())
        with open(self.get_tokenizer_name(), 'wb') as f:
            pickle.dump(self._tokenizer, f, protocol=pickle.HIGHEST_PROTOCOL)


    def load(self):
        self._model = keras.models.load_model(self.get_model_name())
        with open(self.get_tokenizer_name(), 'rb') as f:
            self._tokenizer = pickle.load(f)


    def predict(self, x):
        sequences = self._dataset.text_to_sequence(x)
        return self._model.predict(sequences)

    
    def get_model_name(self):
        return f'./trained_{self._name}.hdf5'

    
    def get_tokenizer_name(self):
        return f'./trained_{self._name}_tokenizer.pickle'
