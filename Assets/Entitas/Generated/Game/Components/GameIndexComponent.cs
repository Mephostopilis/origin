//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public IndexComponent index { get { return (IndexComponent)GetComponent(GameComponentsLookup.Index); } }
    public bool hasIndex { get { return HasComponent(GameComponentsLookup.Index); } }

    public void AddIndex(int newIndex) {
        var index = GameComponentsLookup.Index;
        var component = CreateComponent<IndexComponent>(index);
        component.index = newIndex;
        AddComponent(index, component);
    }

    public void ReplaceIndex(int newIndex) {
        var index = GameComponentsLookup.Index;
        var component = CreateComponent<IndexComponent>(index);
        component.index = newIndex;
        ReplaceComponent(index, component);
    }

    public void RemoveIndex() {
        RemoveComponent(GameComponentsLookup.Index);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherIndex;

    public static Entitas.IMatcher<GameEntity> Index {
        get {
            if (_matcherIndex == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Index);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherIndex = matcher;
            }

            return _matcherIndex;
        }
    }
}
