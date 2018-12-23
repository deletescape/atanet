from enum import Enum


class Language(Enum):
    English = 0
    SimplifiedChinese = 1
    German = 2
    Italian = 3
    French = 4


DefinedLanguageCodes = ['en', 'zh', 'de', 'fr', 'it']


def language_to_string(language: Language) -> str:
    if language == Language.English:
        return 'english'
    elif language == Language.SimplifiedChinese:
        return 'chinese'
    elif language == Language.German:
        return 'german'
    elif language == Language.Italian:
        return 'italian'
    elif language == Language.French:
        return 'french'
