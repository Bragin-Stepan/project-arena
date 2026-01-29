using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Develop.Shared.Const;
using _Project.Develop.Utils;
using UnityEngine;

public class ConfigsLoader
{
    private Dictionary<Type, string> _configsPaths;
    private Dictionary<Type, ScriptableObject> _loadedConfigs;
    private ResourcesLoader _resourcesLoader; 

    public ConfigsLoader(ResourcesLoader resourcesLoader)
    {
        _resourcesLoader = resourcesLoader;
        
        _configsPaths = new Dictionary<Type, string>(ResourcesPath.ScriptableObjects);
        _loadedConfigs = new Dictionary<Type, ScriptableObject>();
    }

    public IEnumerator LoadAsync()
    {
        foreach (KeyValuePair<Type, string> configPath in _configsPaths)
        {
            ScriptableObject config = _resourcesLoader.Load<ScriptableObject>(configPath.Value);
            
            if (config != null)
                _loadedConfigs[configPath.Key] = config;
            
            yield return null;
        }
    }
    
    public T GetConfig<T>() where T : ScriptableObject
    {
        Type type = typeof(T);
        
        if (_loadedConfigs.TryGetValue(type, out ScriptableObject config))
            return config as T;
        
        if (_configsPaths.TryGetValue(type, out string path))
        {
            T loadedConfig = _resourcesLoader.Load<T>(path);
            
            if (loadedConfig != null)
            {
                _loadedConfigs[type] = loadedConfig;
                return loadedConfig;
            }
        }
        
        throw new Exception($"Config of type {typeof(T)} not found at path");
    }
}