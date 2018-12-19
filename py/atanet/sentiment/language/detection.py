from langid import *
from atanet.sentiment.language.language import Language, DefinedLanguageCodes


def detect_language(text: str) -> Language:
    model = langid.model
    language_identifier = langid.LanguageIdentifier.from_modelstring(model)
    language_identifier.set_languages(DefinedLanguageCodes)
    detected = language_identifier.classify(text)
    if detected[0] == 'de':
        return Language.German
    elif detected[0] == 'en':
        return Language.English
    return None

