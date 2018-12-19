from tensorflow import keras
from abc import ABC, abstractmethod
from atanet.sentiment.language.language import Language


class SentimentDataset(ABC):

    @abstractmethod
    def get_data(self):
        return


    @abstractmethod
    def text_to_sequence(self, text: str, tokenizer: keras.preprocessing.text.Tokenizer):
        return


    @abstractmethod
    def get_language(self) -> Language:
        return


    @abstractmethod
    def get_word_count(self) -> int:
        return


    @abstractmethod
    def get_max_x_text_length(self) -> int:
        return

    
    @abstractmethod
    def get_tokenizer(self) -> keras.preprocessing.text.Tokenizer:
        return
