from tensorflow import keras
from abc import ABC, abstractmethod
from atanet.sentiment.language.language import Language
import numpy as np


class SentimentDataset(ABC):

    def __init__(self, max_length_default, test_train_split, loaded_dataset):
        loaded_dataset = loaded_dataset.sample(frac=1).reset_index(drop=True)
        dataset_length = loaded_dataset.shape[0]
        train_count = int(dataset_length * test_train_split)
        x_train = loaded_dataset.loc[:train_count, 'Text'].values
        self.y_train = loaded_dataset.loc[:train_count, 'Sentiment'].values
        x_test = loaded_dataset.loc[(train_count + 1):, 'Text'].values
        self.y_test = loaded_dataset.loc[(train_count + 1):, 'Sentiment'].values
        self.tokenizer = keras.preprocessing.text.Tokenizer()
        total = np.concatenate((x_train, x_test), axis=0)
        self.tokenizer.fit_on_texts(total)
        max_length = max_length_default or max([len(s.split()) for s in total])
        vocab_size = len(self.tokenizer.word_index) + 1
        x_train_tokens =  self.tokenizer.texts_to_sequences(x_train)
        x_test_tokens = self.tokenizer.texts_to_sequences(x_test)
        self.x_train_pad = keras.preprocessing.sequence.pad_sequences(x_train_tokens, maxlen=max_length, padding='post')
        self.x_test_pad = keras.preprocessing.sequence.pad_sequences(x_test_tokens, maxlen=max_length, padding='post')
        self._max_x_length = max_length
        self._vocab_size = vocab_size


    def get_data(self):
        return (self.x_train_pad, self.y_train), (self.x_test_pad, self.y_test)


    @abstractmethod
    def get_language(self) -> Language:
        return


    def get_word_count(self) -> int:
        return self._vocab_size


    def get_max_x_text_length(self) -> int:
        return self._max_x_length

    
    def get_tokenizer(self) -> keras.preprocessing.text.Tokenizer:
        return self.tokenizer
