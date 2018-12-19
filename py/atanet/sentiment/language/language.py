from enum import Enum


class Language(Enum):
    English = 0
    German = 1
    SimplifiedChinese = 2


DefinedLanguageCodes = ['en', 'de', 'zh']


def language_to_string(language: Language) -> str:
    if language == Language.English:
        return 'english'
    elif language == Language.German:
        return 'german'
    elif language == Language.SimplifiedChinese:
        return 'chinese'
