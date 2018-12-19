import tensorflow as tf
from tensorflow import keras
from atanet.sentiment.language.language import Language, language_to_string


class LanguageModel:

    def __init__(self, number_of_words: int, input_length: int, language: Language):
        model = keras.Sequential()
        model.add(keras.layers.Embedding(input_dim=number_of_words, output_dim=64, input_length=input_length))
        model.add(keras.layers.SpatialDropout1D(0.3))
        model.add(keras.layers.Conv1D(activation='relu', padding='same', filters=64, kernel_size=5))
        model.add(keras.layers.MaxPooling1D(pool_size=4))
        model.add(keras.layers.LSTM(250, dropout=0.2, recurrent_dropout=0.2))
        model.add(keras.layers.Dense(1, activation='sigmoid'))
        model.compile(loss='binary_crossentropy', optimizer='adam', metrics=['accuracy'])
        self._model = model
        self._name = language_to_string(language)


    def train(self, x_train, y_train, x_test, y_test):
        self._model.fit(x_train, y_train, validation_data=(x_test, y_test), epochs=3, batch_size=64)
        self._model.save(f'./trained_{self._name}.hdf5')
        scores = self._model.evaluate(x_test, y_test, verbose=0)
        return scores[1]


    def print(self):
        self._model.summary()


    def save(self):
        pass


    def load(self):
        pass


    def predict(self, x):
        return self._model.predict(x)
