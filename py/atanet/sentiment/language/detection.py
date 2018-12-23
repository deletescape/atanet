from langid import *
from atanet.sentiment.language.language import Language, DefinedLanguageCodes


class InvalidLanguageException(Exception):
    
    def __init__(self, language: str):
        self.language = language


def detect_language(text: str) -> Language:
    model = langid.model
    language_identifier = langid.LanguageIdentifier.from_modelstring(model)
    # language_identifier.set_languages(DefinedLanguageCodes)
    detected = language_identifier.classify(text)
    if detected[0] == 'en':
        return Language.English
    elif detected[0] == 'zh':
        return Language.SimplifiedChinese
    elif detected[0] == 'de':
        return Language.German
    elif detected[0] == 'fr':
        return Language.French
    elif detected[0] == 'it':
        return Language.Italian
    else:
        raise InvalidLanguageException(detected[0])
