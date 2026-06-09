# 🔴 Crimson Saver

[![Unity Version](https://img.shields.io/badge/Unity-6000.3%2B-blue?logo=unity)](https://unity.com)
[![Package Version](https://img.shields.io/badge/version-1.0.0-brightgreen)](https://github.com/SoraxDubbing/CrimsonSaver)
[![License](https://img.shields.io/badge/license-MIT-brightgreen.svg)](LICENSE)
[![UPM](https://img.shields.io/badge/UPM-compatible-brightgreen)](https://docs.unity3d.com/Manual/upm-ui.html)
[![Utilisé dans CrimsonSpine](https://img.shields.io/badge/Utilisé%20dans-CrimsonSpine-ff0000)](https://gamagora.itch.io/crimson-spine)

Un système de sauvegarde **abstrait et flexible** pour Unity, permettant de sauvegarder et charger des données facilement avec support pour plusieurs backends de stockage.

> 🎮 **En production** dans [CrimsonSpine](https://gamagora.itch.io/crimson-spine) sur itch.io

---

## 📋 Table des matières

- [Caractéristiques](#-caractéristiques)
- [Pourquoi Crimson Saver ?](#-pourquoi-crimson-saver-)
- [Installation](#-installation)
- [Guide d'utilisation](#-guide-dutilisation)
- [Exemples avancés](#-exemples-avancés)
- [Architecture](#-architecture)
- [Support](#-support)

---

## ✨ Caractéristiques

- 🏗️ **Architecture Pattern Repository** - Interface générique `IRepository<T>` pour une sauvegarde type-safe
- 💾 **Plusieurs backends de stockage** :
  - `JsonRepository<T>` - Sauvegarde JSON structurée
  - `PersistentDataPathSaver` - Fichiers dans le dossier persistant
  - `PlayerPrefsSaver` - Utilisation de PlayerPrefs
- ⚡ **Support asynchrone complet** - `Awaitable` et `Awaitable<T>` pour les opérations non-bloquantes
- 🔑 **Interface ISaver** - Approche clé-valeur flexible
- 📦 **Zero dépendances** - Aucune dépendance externe requise
- 🎯 **Type-safe** - Génériques pour la sécurité des types
- 🧵 **Thread-safe** - Support complet pour les opérations asynchrones sur fond de thread
- 🔀 **Polymorphisme avec [SerializeReference]** - Support des hiérarchies abstraites et classes dérivées
- 📋 **Sérialisation JsonUtility complète** - Supporte tout ce que JsonUtility supporte

---

## 🤔 Pourquoi Crimson Saver ?

### Problèmes résolus

| Problème | Solution |
|----------|----------|
| Code de sauvegarde répétitif | Pattern Repository réutilisable |
| Tight coupling au système de stockage | Interfaces abstraites pour basculer facilement |
| Opérations bloquantes | Support complet async/await |
| Gestion manuelle des clés/chemins | Gestion automatique via `GetKey()` et `KeyFormat()` |
| Type-safety | Génériques `<T>` pour éviter les erreurs de casting |

### Cas d'usage

✅ Systèmes de progression et d'achievements  
✅ Sauvegarde de paramètres et préférences  
✅ Données de joueur (niveaux, stats, inventaire)  
✅ Configurations de jeu  
✅ États de session et contexte persistant  

---

## 📦 Installation

### Via UPM (Unity Package Manager) - Recommandé

#### Option 1 : Depuis URL Git

1. Ouvrez Unity et allez à **Window → TextEditor → Package Manager**
2. Cliquez sur le **➕** en haut à gauche
3. Sélectionnez **Add package from git URL...**
4. Collez l'URL :
   ```
   https://github.com/GamagoRat/unity-CrimsonSaver.git
   ```
5. Appuyez sur **Enter** et attendez l'installation

#### Option 2 : Via manifest.json

Modifiez `Packages/manifest.json` de votre projet :

```json
{
  "dependencies": {
    "fr.phylisiumstudio.crimsonsaver": "https://github.com/GamagoRat/unity-CrimsonSaver.git",
    ...
  }
}
```

#### Option 3 : Depuis une version (stabilisée)

```json
{
  "dependencies": {
    "fr.phylisiumstudio.crimsonsaver": "1.0.0",
    ...
  }
}
```

---

## 🚀 Guide d'utilisation

### Installation de base

Définissez vos données :

```csharp
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int level;
    public float health;
    public int coins;
}
```

### Exemple 1 : Avec JsonRepository et PersistentDataPathSaver

```csharp
using PhylisiumStudio.CrimsonSaver;
using PhylisiumStudio.CrimsonSaver.Repository;
using PhylisiumStudio.CrimsonSaver.Saver;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IRepository<PlayerData> playerRepository;

    private void Start()
    {
        // Créer le saver et le repository
        var saver = new PersistentDataPathSaver("GameData");
        playerRepository = new JsonRepository<PlayerData>(saver);
    }

    public void SavePlayer(PlayerData data)
    {
        playerRepository.Save(data);
        Debug.Log("Données sauvegardées !");
    }

    public void LoadPlayer()
    {
        if (playerRepository.Exists())
        {
            var data = playerRepository.Load();
            Debug.Log($"Joueur chargé : {data.playerName}, Niveau {data.level}");
        }
        else
        {
            Debug.Log("Aucune sauvegarde trouvée");
        }
    }
}
```

### Exemple 2 : Avec PlayerPrefs

```csharp
using PhylisiumStudio.CrimsonSaver;
using PhylisiumStudio.CrimsonSaver.Repository;
using PhylisiumStudio.CrimsonSaver.Extensions; // Pour PlayerPrefsSaver
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private IRepository<GameSettings> settingsRepository;

    [System.Serializable]
    public class GameSettings
    {
        public float masterVolume = 0.8f;
        public bool enableVibration = true;
    }

    private void Start()
    {
        var saver = new PlayerPrefsSaver();
        settingsRepository = new JsonRepository<GameSettings>(saver);
    }

    public void SaveSettings(GameSettings settings)
    {
        settingsRepository.Save(settings);
    }

    public GameSettings LoadSettings()
    {
        return settingsRepository.Load();
    }
}
```

### Exemple 3 : Opérations Asynchrones

```csharp
public class AsyncGameManager : MonoBehaviour
{
    private IRepository<PlayerData> playerRepository;

    private async void SavePlayerAsync(PlayerData data)
    {
        await playerRepository.SaveAsync(data);
        Debug.Log("Données sauvegardées en background !");
    }

    private async void LoadPlayerAsync()
    {
        if (await playerRepository.ExistsAsync())
        {
            var data = await playerRepository.LoadAsync();
            Debug.Log($"Joueur chargé : {data.playerName}");
        }
    }
}
```

### Exemple 4 : Vérifier l'existence des données

```csharp
if (playerRepository.Exists())
{
    Debug.Log("Une sauvegarde existe déjà");
    var existingData = playerRepository.Load();
    // Charger/continuer une partie existante
}
else
{
    Debug.Log("Première création de sauvegarde");
    var newPlayer = new PlayerData { playerName = "Hero", level = 1 };
    playerRepository.Save(newPlayer);
}
```

---

## 🔧 Exemples avancés

### Créer un Saver personnalisé

Implémentez `ISaver` pour votre propre système de stockage :

```csharp
using PhylisiumStudio.CrimsonSaver;
using UnityEngine;

public class CloudSaver : ISaver
{
    public void Save(string key, string data)
    {
        // Implémenter votre logique cloud (Firebase, PlayFab, etc.)
        CloudAPI.SaveData(key, data);
    }

    public string Load(string key)
    {
        return CloudAPI.LoadData(key);
    }

    public bool Exists(string key)
    {
        return CloudAPI.DataExists(key);
    }

    public Awaitable SaveAsync(string key, string data)
    {
        return CloudAPI.SaveDataAsync(key, data);
    }

    public Awaitable<string> LoadAsync(string key)
    {
        return CloudAPI.LoadDataAsync(key);
    }

    public Awaitable<bool> ExistsAsync(string key)
    {
        return CloudAPI.DataExistsAsync(key);
    }

    public string KeyFormat(string key)
    {
        return $"cloud_{key}";
    }
}
```

Puis utilisez-le :

```csharp
var cloudSaver = new CloudSaver();
var cloudRepository = new JsonRepository<PlayerData>(cloudSaver);
```

### Stratégie multi-tier (local + cloud)

```csharp
public class HybridSaveManager : MonoBehaviour
{
    private IRepository<PlayerData> localRepository;
    private IRepository<PlayerData> cloudRepository;

    private void Start()
    {
        localRepository = new JsonRepository<PlayerData>(
            new PersistentDataPathSaver("LocalData")
        );
        cloudRepository = new JsonRepository<PlayerData>(
            new CloudSaver()
        );
    }

    public async void SaveToCloud(PlayerData data)
    {
        // Sauvegarder localement d'abord
        localRepository.Save(data);
        
        // Puis synchroniser au cloud en arrière-plan
        await cloudRepository.SaveAsync(data);
        Debug.Log("Données synchronisées au cloud");
    }
}
```

---

## 🏛️ Architecture

### Diagramme des interfaces

```
┌─────────────────────────────────┐
│     IRepository<T>              │  ← Interface générique
├─────────────────────────────────┤
│ + Save(T data)                  │
│ + Load() : T                    │
│ + Exists() : bool               │
│ + SaveAsync(T) : Awaitable      │
│ + LoadAsync() : Awaitable<T>    │
│ + ExistsAsync() : Awaitable<bool>
└─────────────────────────────────┘
           ▲
           │ implements
           │
      ┌────────────────┐
      │ JsonRepository │  ← Implémentation concrète
      └────────────────┘
           │ utilise
           ▼
┌─────────────────────────────────┐
│        ISaver                   │  ← Interface de stockage
├─────────────────────────────────┤
│ + Save(key, data)               │
│ + Load(key) : string            │
│ + Exists(key) : bool            │
│ + KeyFormat(key) : string       │
│ + *Async variants               │
└─────────────────────────────────┘
     ▲           ▲           ▲
     │           │           │
   Implémentations:
   ├─ PersistentDataPathSaver
   ├─ PlayerPrefsSaver
   └─ CloudSaver (personnalisé)
```

### Flux de données

```
PlayerData
    │
    ▼
Repository.Save(data)
    │
    ├─→ JsonExtension.ToJson(data)     [sérialisation]
    │
    └─→ Saver.Save(key, jsonString)    [stockage]
        │
        └─→ Backend (fichier, PlayerPrefs, cloud, etc.)
```

---

## ⚙️ Configuration

### Noms de clés personnalisés

Par défaut, la clé est le nom du type. Vous pouvez l'override :

```csharp
public class CustomRepository<T> : JsonRepository<T>
{
    public override string GetKey() => "MyCustomKey";
}
```

### Chemins de dossier personnalisés

```csharp
// Sauvegarde dans un sous-dossier
var saver = new PersistentDataPathSaver("MyGame/Saves");
```

---

## 📋 Spécifications

| Aspect | Détail |
|--------|--------|
| **Version minimale Unity** | 6000.3 |
| **Version du package** | 1.0.0 |
| **Namespace** | `PhylisiumStudio.CrimsonSaver` |
| **Dépendances** | Aucune |
| **Licence** | MIT |
| **Plateforme** | All (Windows, Mac, Linux, iOS, Android, WebGL) |

---

## 🐛 Support

Des problèmes ? Consultez :

- 📖 [Documentation Unity JsonUtility](https://docs.unity3d.com/ScriptReference/JsonUtility.html)
- 💾 [Documentation PlayerPrefs](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html)
- 🚀 [UPM Guide](https://docs.unity3d.com/Manual/upm-ui.html)

### Sérialisation avancée avec [SerializeReference]

Pour sérialiser des listes de **modèles abstraits** ou des **hiérarchies polymorphes**, utilisez l'attribut `[SerializeReference]` :

```csharp
[System.Serializable]
public abstract class Effect
{
    public string name;
}

[System.Serializable]
public class DamageEffect : Effect
{
    public int damage;
}

[System.Serializable]
public class HealEffect : Effect
{
    public int healAmount;
}

[System.Serializable]
public class SkillData
{
    public string skillName;
    
    [SerializeReference]  // ← Clé pour polymorphisme
    public List<Effect> effects = new List<Effect>();
}

// Utilisation
var skillData = new SkillData 
{ 
    skillName = "Combo",
    effects = new List<Effect>
    {
        new DamageEffect { name = "Hit", damage = 25 },
        new HealEffect { name = "Lifesteal", healAmount = 5 }
    }
};

var repository = new JsonRepository<SkillData>(
    new PersistentDataPathSaver("Skills")
);
repository.Save(skillData);
```

### Limitations connues

⚠️ **JsonUtility** limitations :
- Dictionnaires ne sont pas sérialisables directement → Utilisez une classe wrapper avec arrays
- Tableaux génériques `List<T>` nécessitent une classe wrapper pour certains types

**Solution** :

```csharp
[System.Serializable]
public class IntList
{
    public int[] items;
}

[System.Serializable]
public class GameData
{
    public IntList scores = new IntList();  // Wrapper requis
}
```

**Cependant** : Crimson Saver supporte **tout ce que JsonUtility supporte**, y compris :
- Types primitifs (int, float, string, bool, etc.)
- Classes [Serializable]
- Arrays
- [SerializeReference] pour polymorphisme
- Nested classes

---

## 📝 Licence

Ce projet est sous licence **MIT**. Voir le fichier [LICENSE](LICENSE) pour plus de détails.

---

## 👨‍💻 Auteur

Créé par **[SoraxDubbing](https://sorax5.github.io/)** durant la formation Gamagora pour le projet [CrimsonSpine](https://gamagora.itch.io/crimson-spine)

---

## ✅ Capacités de sérialisation

Crimson Saver supporte **tout ce que [JsonUtility](https://docs.unity3d.com/ScriptReference/JsonUtility.html) supporte**, notamment :

| Type | Supporté | Notes |
|------|----------|-------|
| Types primitifs (int, float, string, bool) | ✅ | Soutien natif |
| Classes [Serializable] | ✅ | Recommandé |
| Arrays (T[]) | ✅ | Soutien natif |
| Nested classes | ✅ | Classes imbriquées |
| [SerializeReference] polymorphisme | ✅ | Pour hiérarchies abstraites |
| List<T> (wrapper) | ✅ | Via classe conteneur |
| Dictionnaires | ❌ | Utilisez des wrappers |

---

## 🎯 Roadmap

- [x] Support JsonUtility complet (✅ Terminé)
- [x] Support [SerializeReference] (✅ Terminé)
- [ ] Chiffrement optionnel des données
- [ ] Versioning automatique des sauvegardes

---

<div align="center">

**[⬆ Retour au sommet](#-crimson-saver)**

Fait avec ❤️ pour la communauté Gamagorat

</div>
