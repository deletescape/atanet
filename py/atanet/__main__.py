from argparse import ArgumentParser


if __name__ == '__main__':
    parser = ArgumentParser()
    parser.add_argument('command')
    args = parser.parse_args()
    if args.command == 'train':
        from atanet.sentiment.model import LanguageModel
        from atanet.sentiment.datasets.english_twitter import EnglishTwitterDataset
        from atanet.sentiment.datasets.simplified_chinese import SimplifiedChineseDataset

        chinese_dataset = SimplifiedChineseDataset()
        chinese_model = LanguageModel(chinese_dataset)
        chinese_model.epochs = 5

        english_dataset = EnglishTwitterDataset()
        english_model = LanguageModel(english_dataset)
        english_model.epochs = 1

        models = [chinese_model, english_model]
        for model in models:
            model.train()
            model.save()

    elif args.command == 'serve':
        from atanet.api.model_server import run
        run()
    else:
        raise ValueError('Invalid command line option')
