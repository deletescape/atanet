from atanet.sentiment.datasets.set import SentimentDataset
from atanet.sentiment.language.language import Language
from tensorflow import keras
from nltk import word_tokenize


TOP_WORDS = 15000
MAX_REVIEW_LENGTH = 100


class EnglishDataset(SentimentDataset):

    def __init__(self):
        self._word_index = keras.datasets.imdb.get_word_index()


    def get_data(self):
        (x_train, y_train), (x_test, y_test) = keras.datasets.imdb.load_data(num_words=TOP_WORDS)
        x_train = keras.preprocessing.sequence.pad_sequences(x_train, maxlen=MAX_REVIEW_LENGTH, padding='post')
        x_test = keras.preprocessing.sequence.pad_sequences(x_test, maxlen=MAX_REVIEW_LENGTH, padding='post')
        return (x_train, y_train), (x_test, y_test)


    def text_to_sequence(self, text: str):
        item = []
        for word in word_tokenize(text.lower()):
            # TODO: check if adding 0's instead of nothing improves results
            if word in self._word_index and self._word_index[word] < TOP_WORDS:
                item.append(self._word_index[word])
        return keras.preprocessing.sequence.pad_sequences([item], maxlen=MAX_REVIEW_LENGTH)


    def get_language(self) -> Language:
        return Language.English


    def get_word_count(self) -> int:
        return TOP_WORDS


    def get_max_x_text_length(self) -> int:
        return MAX_REVIEW_LENGTH
