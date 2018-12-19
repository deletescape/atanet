from atanet.sentiment.datasets.set import SentimentDataset
from atanet.sentiment.language.language import Language
from tensorflow import keras
from nltk import word_tokenize
import pandas as pd
import numpy as np
import zipfile
import os


TEST_TRAIN_SPLIT = 0.85
MAX_LENGTH = 100


class EnglishTwitterDataset(SentimentDataset):

    def __init__(self):
        columns = ['Sentiment', 'Id', 'Date', 'Flag', 'User', 'Text']
        path = './atanet/sentiment/datasets/twitter_english.csv'
        zip_path = './atanet/sentiment/datasets/twitter_english.zip'
        if not os.path.isfile(path):
            with zipfile.ZipFile(zip_path, 'r') as zip_ref:
                zip_ref.extractall('./atanet/sentiment/datasets/')
            os.rename('./atanet/sentiment/datasets/training.1600000.processed.noemoticon.csv', path)
        self._loaded: pd.DataFrame = pd.read_csv(path, encoding='ISO-8859-1', names=columns)
        self._loaded = self._loaded.sample(frac=1).reset_index(drop=True)
        self._loaded.Sentiment /= 4
        dataset_length = self._loaded.shape[0]
        train_count = int(dataset_length * TEST_TRAIN_SPLIT)
        x_train = self._loaded.loc[:train_count, 'Text'].values
        self.y_train = self._loaded.loc[:train_count, 'Sentiment'].values
        x_test = self._loaded.loc[(train_count + 1):, 'Text'].values
        self.y_test = self._loaded.loc[(train_count + 1):, 'Sentiment'].values
        self.tokenizer = keras.preprocessing.text.Tokenizer()
        total = np.concatenate((x_train, x_test), axis=0)
        self.tokenizer.fit_on_texts(total)
        max_length = MAX_LENGTH or max([len(s.split()) for s in total])
        vocab_size = len(self.tokenizer.word_index) + 1
        x_train_tokens =  self.tokenizer.texts_to_sequences(x_train)
        x_test_tokens = self.tokenizer.texts_to_sequences(x_test)
        self.x_train_pad = keras.preprocessing.sequence.pad_sequences(x_train_tokens, maxlen=max_length, padding='post')
        self.x_test_pad = keras.preprocessing.sequence.pad_sequences(x_test_tokens, maxlen=max_length, padding='post')
        self._max_x_length = max_length
        self._vocab_size = vocab_size


    def get_data(self):
        return (self.x_train_pad, self.y_train), (self.x_test_pad, self.y_test)


    def text_to_sequence(self, text: str, tokenizer: keras.preprocessing.text.Tokenizer):
        sequences = tokenizer.texts_to_sequences([text])
        return keras.preprocessing.sequence.pad_sequences(sequences, maxlen=self._max_x_length, padding='post')


    def get_language(self) -> Language:
        return Language.English


    def get_word_count(self) -> int:
        return self._vocab_size


    def get_max_x_text_length(self) -> int:
        return self._max_x_length


    def get_tokenizer(self) -> keras.preprocessing.text.Tokenizer:
        return self.tokenizer
