export class AppConfig {
    public baseUrl: string | undefined = undefined;
}

export class ConfigService {
    private settings: AppConfig | undefined = undefined;

    public get config(): AppConfig {
        return this.settings;
    }

    public loadConfig(): Promise<any> {
        return new Promise(async (resolve, _) => {
            const response = await fetch('./assets/configs/config.json');
            const remoteConfig: AppConfig = await response.json();
            this.settings = remoteConfig;
            resolve(remoteConfig);
        });
    }
}
