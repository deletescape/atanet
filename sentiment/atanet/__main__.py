import sys
from argparse import ArgumentParser


if __name__ == '__main__':
    parser = ArgumentParser()
    parser.add_argument('--run-server')
    parser.add_argument('--train')
    args = parser.parse_args(sys.argv)
    if args.train:
        pass
    else:
        pass
