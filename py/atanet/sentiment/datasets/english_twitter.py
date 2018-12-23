from atanet.sentiment.datasets.set import SentimentDataset
from atanet.sentiment.language.language import Language
import pandas as pd
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
        loaded: pd.DataFrame = pd.read_csv(path, encoding='ISO-8859-1', names=columns)
        loaded.Sentiment /= 4
        super().__init__(MAX_LENGTH, TEST_TRAIN_SPLIT, loaded)


    def get_language(self) -> Language:
        return Language.English
