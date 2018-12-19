import nltk
from argparse import ArgumentParser
from atanet.sentiment.model import LanguageModel
from atanet.sentiment.datasets.english_twitter import EnglishTwitterDataset
from atanet.sentiment.language.language import Language


def maybe_download_nltk_dependencies():
    nltk.download('punkt')


if __name__ == '__main__':
    parser = ArgumentParser()
    parser.add_argument('command')
    args = parser.parse_args()
    if args.command == 'train':
        dataset = EnglishTwitterDataset()
        model = LanguageModel(dataset.get_word_count(), dataset.get_max_x_text_length(), dataset.get_language())
        (x_train, y_train), (x_test, y_test) = dataset.get_data()
        model.train(x_train, y_train, x_test, y_test)
        while True:
            i = input('Text: ')
            seq = dataset.text_to_sequence(i)
            print(seq)
            res = model.predict(seq)
            print(res)
    elif args.command == 'serve':
        pass
    else:
        raise ValueError('Invalid command line option')