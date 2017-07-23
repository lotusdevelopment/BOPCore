(function () {
    $(function () {
        if ($lf.ss.us.User.UserLanguage != "en") translateFromBeginning();
        return;
    });
    function translateCustom(text, languageTo) {

    }
    function translateFromBeginning() {
        var phrases = $(".trnlstxt");
        var dictionary = $lf.parseString(getWords($lf.ss.us.User.UserLanguage));
        $.each(phrases, function (i, j) {
            var th = this;
            var currentWord = getWordFromField(th);
            if (IsNullOrEmptyString(currentWord)) return;
            currentWord = $.trim(currentWord);
            var existingWord = SearchValue(currentWord, dictionary);
            if (!IsNullOrEmptyString(existingWord)) { replaceTranslatedText(existingWord, th); return; }            
            $.ajax({
                url: $lf.ss.cs.Company + "Translate/" + currentWord + "/en" + "/" + $lf.ss.us.User.UserLanguage,
                success: function (data) {
                    var obj = {};
                    obj[currentWord] = data;
                    dictionary.push(obj);
                    var language = $lf.ss.us.User.UserLanguage;
                    localStorage.setItem(language, $lf.parseObject(dictionary));
                    replaceTranslatedText(data, th);
                },
                dataType: "json",
                error: function (xhr) {
                    console.log($lf.parseObject(xhr));
                },
                async: true
            });
        });
    }
    function replaceTranslatedText(data, th) {
        var intrig = th.childNodes;
        $.each(intrig, function (k, l) {
            if (l.nodeName == "#text") l.textContent = data;
        })
    }
    function setLanguagedictionary() {
        var dct = {};
        return getLanguagesDictionary($lf.ss.us.User.UserLanguage);
    }
    function getLanguagesDictionary(language) {
        var result = "";
        $.ajax({
            url: $lf.ss.cs.Company + "GetFullDictionary/" + language,
            async: false,
            success: function (data) {
                var arr = [];
                $.each(data, function (i, j) {
                    var obj = {};
                    obj[j.Key] = j.Value;
                    arr.push(obj);
                });
                result = arr;
            }
        });
        return result;
    }
    function getWords(language) {
        var langi = localStorage.getItem(language);
        if (langi == null || langi == undefined || langi == "undefined")
            localStorage.setItem(language, $lf.parseObject(setLanguagedictionary(language)));
        return localStorage.getItem(language);
    }
    function SearchValue(word, dct) {
        var rtn = null;
        $.each(dct, function (i, j) {
            var key = Object.keys(j);
            var value = Object.values(j);
            if (word == key) { rtn = Object.values(j); }
        });
        return rtn;
    }
    function getWordFromField(j) {
        var intrig = j.childNodes;
        var rtn = null;
        $.each(intrig, function (k, l) {
            if (l.nodeName == "#text") rtn = l.textContent;
        })
        return rtn;
    }
})();