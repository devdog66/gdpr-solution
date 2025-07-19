import { CookiePreferences } from "../domain/models";

export class CookiePrefsPresenter {

    savePreferences(cookiePrefs: CookiePreferences) {
        if(cookiePrefs != null){}
    }

    getPreferences(): Promise<CookiePreferences> {

        return new Promise<CookiePreferences>(async (resolve) => {
            let prefs = new CookiePreferences();

            /*let clientConfig = ClientConfig.getInstance();
            let appPresenter = AppPresenter.getInstance(clientConfig);
    
            await appPresenter.checkCookieConsent();
    
            let sessionState = new SessionState();
            let storedPrefs = sessionState.getCookiePrefs();
            if (storedPrefs != null) {
                resolve(storedPrefs);
                return;
            }*/

            resolve(prefs);
        });
    }

    openPrefs(itemName: string) {
        var elems = ['yourPrivacy', 'necessary', 'tracking', 'advertising'];
        for (var idx in elems) {
            var elName = elems[idx];
            var el = document.getElementById(elName);
            if (el != null) {
                el.classList.add("w3-hide");
            }
        }
        var el = document.getElementById(itemName);
        if (el != null) {
            el.classList.remove("w3-hide");
        }
    }
}