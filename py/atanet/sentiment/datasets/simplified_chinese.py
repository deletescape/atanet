from atanet.sentiment.datasets.set import SentimentDataset
from atanet.sentiment.language.language import Language
from tensorflow import keras
from nltk import word_tokenize
import pandas as pd
import os


TEST_TRAIN_SPLIT = 0.6
MAX_LENGTH = 50


class SimplifiedChineseDataset(SentimentDataset):

    def __init__(self):
        path = './atanet/sentiment/datasets/conversation_chinese.csv'
        df = pd.read_csv(path, names=['Sentiment', 'Text'])
        df['Text'] = df['Text'].astype(str)
        super().__init__(MAX_LENGTH, TEST_TRAIN_SPLIT, df)


    def get_language(self) -> Language:
        return Language.SimplifiedChinese
