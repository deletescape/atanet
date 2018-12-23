import os
from flask import Flask, request, make_response, jsonify
from atanet.sentiment.language.detection import detect_language, InvalidLanguageException
from atanet.sentiment.model import LanguageModel
from atanet.sentiment.datasets.english_twitter import EnglishTwitterDataset
from atanet.sentiment.language.language import Language
from atanet.api.external import pretrained_predict


app = Flask(__name__)

loaded_english_model = LanguageModel.create_loaded(Language.English)
loaded_english_model.predict([])

loaded_chinese_model = LanguageModel.create_loaded(Language.SimplifiedChinese)
loaded_chinese_model.predict([])

self_trained = {
    Language.English: loaded_english_model,
    Language.SimplifiedChinese: loaded_chinese_model
}


def load_and_predict(lang: Language):
    def fn(text: str):
        model = self_trained[lang]
        result = model.predict([text])
        return float(result[0][0])
    return fn

def avg_predict_en(text: str):
    cur_res = load_and_predict(Language.English)(text)
    pretrained_res = pretrained_predict(text, Language.English)
    return (cur_res + pretrained_res) / 2


models = {
    Language.English: avg_predict_en,
    Language.SimplifiedChinese: load_and_predict(Language.SimplifiedChinese),
    Language.German: lambda x: pretrained_predict(x, Language.German),
    Language.Italian: lambda x: pretrained_predict(x, Language.Italian),
    Language.French: lambda x: pretrained_predict(x, Language.French)
}

@app.route('/')
def predict():
    global models
    text = request.args.get('text')
    if text is None:
        return make_response(jsonify({
            'success': False,
            'message': 'No text provided'
        }), 400)

    language = None
    try:
        language = detect_language(text)
    except InvalidLanguageException as ex:
        return make_response(jsonify({
            'success': False,
            'message': f'Invalid language: {ex.language}'
        }), 400)

    if language not in models:
        return make_response(jsonify({
            'success': False,
            'message': 'No model for provided language available'
        }), 400)

    res = models[language](text)
    return make_response(jsonify({
        'sentiment': res
    }), 200)


def run():
    key = 'ASPNETCORE_ENVIRONMENT'
    debug = os.environ[key] == 'Development' if key in os.environ else False
    app.run(host='0.0.0.0', port=os.environ['SENTIMENT_PORT'], debug=debug)
