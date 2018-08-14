MgrScene = {}

screenWidth = nil
screenHeight = nil
screenScale = nil
isSceneChanging = nil
isSceneOrPopupChanging = nil
useGUILayout = nil
runInEditMode = nil
enabled = nil
isActiveAndEnabled = nil
transform = nil
gameObject = nil
tag = nil
rigidbody = nil
rigidbody2D = nil
camera = nil
light = nil
animation = nil
constantForce = nil
renderer = nil
audio = nil
guiText = nil
networkView = nil
guiElement = nil
guiTexture = nil
collider = nil
collider2D = nil
hingeJoint = nil
particleEmitter = nil
particleSystem = nil
name = nil
hideFlags = nil
function MgrScene.Init () 
end

function MgrScene.SetViewRoot (grp) 
end

function MgrScene.GetViewRoot () 
end

function MgrScene.GetRoot () 
end

function MgrScene.GetViewRootCanvas () 
end

function MgrScene.GetCurrentScene () 
end

function MgrScene.GetAllScenes () 
end

function MgrScene.LoadScene (sceneName, data) 
end

function MgrScene.LoadScene (sceneName) 
end

function MgrScene.DestroyScene (sceneObj, destroyImmediate) 
end

function MgrScene.DestroyScene (sceneName, destroyImmediate) 
end

function MgrScene.GetScene (sceneName) 
end

function MgrScene.GotoScene (par) 
end

function MgrScene.GotoScene (sceneName) 
end

function MgrScene.GotoScene (sceneName, data) 
end

function MgrScene.GotoScene (sceneName, anim) 
end

function MgrScene.GotoScene (sceneName, oldSceneTime, newSceneTime) 
end

function MgrScene.GotoScene (sceneName, eachSceneTime) 
end

function MgrScene.GotoScene (sceneName, ease) 
end

function MgrScene.GotoScene (sceneName, cb) 
end

function MgrScene.GotoScene (sceneName, data, anim) 
end

function MgrScene.GotoScene (sceneName, data, oldSceneTime, newSceneTime) 
end

function MgrScene.GotoScene (sceneName, data, eachSceneTime) 
end

function MgrScene.GotoScene (sceneName, data, ease) 
end

function MgrScene.GotoScene (sceneName, data, cb) 
end

function MgrScene.GotoScene (sceneName, anim, oldSceneTime, newSceneTime) 
end

function MgrScene.GotoScene (sceneName, anim, eachSceneTime) 
end

function MgrScene.GotoScene (sceneName, anim, ease) 
end

function MgrScene.GotoScene (sceneName, anim, cb) 
end

function MgrScene.GotoScene (sceneName, oldSceneTime, newSceneTime, cb) 
end

function MgrScene.GotoScene (sceneName, oldSceneTime, newSceneTime, ease) 
end

function MgrScene.GotoScene (sceneName, eachSceneTime, ease) 
end

function MgrScene.GotoScene (sceneName, eachSceneTime, cb) 
end

function MgrScene.GotoScene (sceneName, ease, cb) 
end

function MgrScene.GotoScene (sceneName, data, anim, oldSceneTime, newSceneTime) 
end

function MgrScene.GotoScene (sceneName, data, anim, eachSceneTime) 
end

function MgrScene.GotoScene (sceneName, data, anim, ease) 
end

function MgrScene.GotoScene (sceneName, data, anim, cb) 
end

function MgrScene.GotoScene (sceneName, data, oldSceneTime, newSceneTime, ease) 
end

function MgrScene.GotoScene (sceneName, data, oldSceneTime, newSceneTime, cb) 
end

function MgrScene.GotoScene (sceneName, data, eachSceneTime, ease) 
end

function MgrScene.GotoScene (sceneName, data, eachSceneTime, cb) 
end

function MgrScene.GotoScene (sceneName, data, ease, cb) 
end

function MgrScene.GotoScene (sceneName, anim, oldSceneTime, newSceneTime, ease) 
end

function MgrScene.GotoScene (sceneName, anim, oldSceneTime, newSceneTime, cb) 
end

function MgrScene.GotoScene (sceneName, anim, eachSceneTime, ease) 
end

function MgrScene.GotoScene (sceneName, anim, eachSceneTime, cb) 
end

function MgrScene.GotoScene (sceneName, anim, ease, cb) 
end

function MgrScene.GotoScene (sceneName, oldSceneTime, newSceneTime, ease, cb) 
end

function MgrScene.GotoScene (sceneName, eachSceneTime, ease, cb) 
end

function MgrScene.GotoScene (sceneName, data, anim, oldSceneTime, newSceneTime, ease) 
end

function MgrScene.GotoScene (sceneName, data, anim, oldSceneTime, newSceneTime, cb) 
end

function MgrScene.GotoScene (sceneName, data, anim, eachSceneTime, ease) 
end

function MgrScene.GotoScene (sceneName, data, anim, eachSceneTime, cb) 
end

function MgrScene.GotoScene (sceneName, data, anim, ease, cb) 
end

function MgrScene.GotoScene (sceneName, data, oldSceneTime, newSceneTime, ease, cb) 
end

function MgrScene.GotoScene (sceneName, data, eachSceneTime, ease, cb) 
end

function MgrScene.GotoScene (sceneName, anim, oldSceneTime, newSceneTime, ease, cb) 
end

function MgrScene.GotoScene (sceneName, anim, eachSceneTime, ease, cb) 
end

function MgrScene.GotoScene (sceneName, data, anim, oldSceneTime, newSceneTime, ease, cb) 
end

function MgrScene.GotoScene (sceneName, data, anim, eachSceneTime, ease, cb) 
end

function MgrScene.Update () 
end

function MgrScene:Invoke (methodName, time) 
end

function MgrScene:InvokeRepeating (methodName, time, repeatRate) 
end

function MgrScene:CancelInvoke () 
end

function MgrScene:CancelInvoke (methodName) 
end

function MgrScene:IsInvoking (methodName) 
end

function MgrScene:IsInvoking () 
end

function MgrScene:StartCoroutine (routine) 
end

function MgrScene:StartCoroutine_Auto (routine) 
end

function MgrScene:StartCoroutine (methodName, value) 
end

function MgrScene:StartCoroutine (methodName) 
end

function MgrScene:StopCoroutine (methodName) 
end

function MgrScene:StopCoroutine (routine) 
end

function MgrScene:StopCoroutine (routine) 
end

function MgrScene:StopAllCoroutines () 
end

function MgrScene:GetComponent (type) 
end

function MgrScene:GetComponent () 
end

function MgrScene:GetComponent (type) 
end

function MgrScene:GetComponentInChildren (t, includeInactive) 
end

function MgrScene:GetComponentInChildren (t) 
end

function MgrScene:GetComponentInChildren () 
end

function MgrScene:GetComponentInChildren (includeInactive) 
end

function MgrScene:GetComponentsInChildren (t) 
end

function MgrScene:GetComponentsInChildren (t, includeInactive) 
end

function MgrScene:GetComponentsInChildren (includeInactive) 
end

function MgrScene:GetComponentsInChildren (includeInactive, result) 
end

function MgrScene:GetComponentsInChildren () 
end

function MgrScene:GetComponentsInChildren (results) 
end

function MgrScene:GetComponentInParent (t) 
end

function MgrScene:GetComponentInParent () 
end

function MgrScene:GetComponentsInParent (t) 
end

function MgrScene:GetComponentsInParent (t, includeInactive) 
end

function MgrScene:GetComponentsInParent (includeInactive) 
end

function MgrScene:GetComponentsInParent (includeInactive, results) 
end

function MgrScene:GetComponentsInParent () 
end

function MgrScene:GetComponents (type) 
end

function MgrScene:GetComponents (type, results) 
end

function MgrScene:GetComponents (results) 
end

function MgrScene:GetComponents () 
end

function MgrScene:CompareTag (tag) 
end

function MgrScene:SendMessageUpwards (methodName, value, options) 
end

function MgrScene:SendMessageUpwards (methodName, value) 
end

function MgrScene:SendMessageUpwards (methodName) 
end

function MgrScene:SendMessageUpwards (methodName, options) 
end

function MgrScene:SendMessage (methodName, value, options) 
end

function MgrScene:SendMessage (methodName, value) 
end

function MgrScene:SendMessage (methodName) 
end

function MgrScene:SendMessage (methodName, options) 
end

function MgrScene:BroadcastMessage (methodName, parameter, options) 
end

function MgrScene:BroadcastMessage (methodName, parameter) 
end

function MgrScene:BroadcastMessage (methodName) 
end

function MgrScene:BroadcastMessage (methodName, options) 
end

function MgrScene:ToString () 
end

function MgrScene:GetInstanceID () 
end

function MgrScene:GetHashCode () 
end

function MgrScene:Equals (other) 
end

function MgrScene:GetType () 
end

