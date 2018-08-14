MgrPopup = {}

isPopupChanging = nil
isSceneOrPopupChanging = nil
hasPopup = nil
popupCount = nil
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
function MgrPopup.Init () 
end

function MgrPopup.SetPopupRoot (grp) 
end

function MgrPopup.GetPopupRoot () 
end

function MgrPopup.GetPopupRootCanvas () 
end

function MgrPopup.GetPopupViewVisiblity () 
end

function MgrPopup.SetPopupViewVisiblity (visiblity) 
end

function MgrPopup.GetAllPopups () 
end

function MgrPopup.LoadPopup (sceneName, data) 
end

function MgrPopup.LoadPopup (sceneName) 
end

function MgrPopup.GetPopup (sceneName) 
end

function MgrPopup.HideAll () 
end

function MgrPopup.HideTop () 
end

function MgrPopup.GetTop () 
end

function MgrPopup.Show (par) 
end

function MgrPopup.Show (sceneName) 
end

function MgrPopup.Show (sceneName, data) 
end

function MgrPopup.Show (sceneName, anim) 
end

function MgrPopup.Show (sceneName, oldSceneTime, newSceneTime) 
end

function MgrPopup.Show (sceneName, eachSceneTime) 
end

function MgrPopup.Show (sceneName, ease) 
end

function MgrPopup.Show (sceneName, cb) 
end

function MgrPopup.Show (sceneName, data, anim) 
end

function MgrPopup.Show (sceneName, data, oldSceneTime, newSceneTime) 
end

function MgrPopup.Show (sceneName, data, eachSceneTime) 
end

function MgrPopup.Show (sceneName, data, ease) 
end

function MgrPopup.Show (sceneName, data, cb) 
end

function MgrPopup.Show (sceneName, anim, oldSceneTime, newSceneTime) 
end

function MgrPopup.Show (sceneName, anim, eachSceneTime) 
end

function MgrPopup.Show (sceneName, anim, ease) 
end

function MgrPopup.Show (sceneName, anim, cb) 
end

function MgrPopup.Show (sceneName, oldSceneTime, newSceneTime, cb) 
end

function MgrPopup.Show (sceneName, oldSceneTime, newSceneTime, ease) 
end

function MgrPopup.Show (sceneName, eachSceneTime, ease) 
end

function MgrPopup.Show (sceneName, eachSceneTime, cb) 
end

function MgrPopup.Show (sceneName, ease, cb) 
end

function MgrPopup.Show (sceneName, data, anim, oldSceneTime, newSceneTime) 
end

function MgrPopup.Show (sceneName, data, anim, eachSceneTime) 
end

function MgrPopup.Show (sceneName, data, anim, ease) 
end

function MgrPopup.Show (sceneName, data, anim, cb) 
end

function MgrPopup.Show (sceneName, data, oldSceneTime, newSceneTime, ease) 
end

function MgrPopup.Show (sceneName, data, oldSceneTime, newSceneTime, cb) 
end

function MgrPopup.Show (sceneName, data, eachSceneTime, ease) 
end

function MgrPopup.Show (sceneName, data, eachSceneTime, cb) 
end

function MgrPopup.Show (sceneName, data, ease, cb) 
end

function MgrPopup.Show (sceneName, anim, oldSceneTime, newSceneTime, ease) 
end

function MgrPopup.Show (sceneName, anim, oldSceneTime, newSceneTime, cb) 
end

function MgrPopup.Show (sceneName, anim, eachSceneTime, ease) 
end

function MgrPopup.Show (sceneName, anim, eachSceneTime, cb) 
end

function MgrPopup.Show (sceneName, anim, ease, cb) 
end

function MgrPopup.Show (sceneName, oldSceneTime, newSceneTime, ease, cb) 
end

function MgrPopup.Show (sceneName, eachSceneTime, ease, cb) 
end

function MgrPopup.Show (sceneName, data, anim, oldSceneTime, newSceneTime, ease) 
end

function MgrPopup.Show (sceneName, data, anim, oldSceneTime, newSceneTime, cb) 
end

function MgrPopup.Show (sceneName, data, anim, eachSceneTime, ease) 
end

function MgrPopup.Show (sceneName, data, anim, eachSceneTime, cb) 
end

function MgrPopup.Show (sceneName, data, anim, ease, cb) 
end

function MgrPopup.Show (sceneName, data, oldSceneTime, newSceneTime, ease, cb) 
end

function MgrPopup.Show (sceneName, data, eachSceneTime, ease, cb) 
end

function MgrPopup.Show (sceneName, anim, oldSceneTime, newSceneTime, ease, cb) 
end

function MgrPopup.Show (sceneName, anim, eachSceneTime, ease, cb) 
end

function MgrPopup.Show (sceneName, data, anim, oldSceneTime, newSceneTime, ease, cb) 
end

function MgrPopup.Show (sceneName, data, anim, eachSceneTime, ease, cb) 
end

function MgrPopup.Hide (par) 
end

function MgrPopup.Hide (sceneName) 
end

function MgrPopup.Hide (sceneName, anim) 
end

function MgrPopup.Hide (sceneName, oldSceneTime, newSceneTime) 
end

function MgrPopup.Hide (sceneName, eachSceneTime) 
end

function MgrPopup.Hide (sceneName, ease) 
end

function MgrPopup.Hide (sceneName, cb) 
end

function MgrPopup.Hide (sceneName, anim, oldSceneTime, newSceneTime) 
end

function MgrPopup.Hide (sceneName, anim, eachSceneTime) 
end

function MgrPopup.Hide (sceneName, anim, ease) 
end

function MgrPopup.Hide (sceneName, anim, cb) 
end

function MgrPopup.Hide (sceneName, oldSceneTime, newSceneTime, ease) 
end

function MgrPopup.Hide (sceneName, oldSceneTime, newSceneTime, cb) 
end

function MgrPopup.Hide (sceneName, eachSceneTime, ease) 
end

function MgrPopup.Hide (sceneName, eachSceneTime, cb) 
end

function MgrPopup.Hide (sceneName, ease, cb) 
end

function MgrPopup.Hide (sceneName, anim, oldSceneTime, newSceneTime, ease) 
end

function MgrPopup.Hide (sceneName, anim, eachSceneTime, ease) 
end

function MgrPopup.Hide (sceneName, anim, oldSceneTime, newSceneTime, cb) 
end

function MgrPopup.Hide (sceneName, anim, eachSceneTime, cb) 
end

function MgrPopup.Hide (sceneName, anim, ease, cb) 
end

function MgrPopup.Hide (sceneName, oldSceneTime, newSceneTime, ease, cb) 
end

function MgrPopup.Hide (sceneName, eachSceneTime, ease, cb) 
end

function MgrPopup.Hide (sceneName, anim, oldSceneTime, newSceneTime, ease, cb) 
end

function MgrPopup.Hide (sceneName, anim, eachSceneTime, ease, cb) 
end

function MgrPopup:Invoke (methodName, time) 
end

function MgrPopup:InvokeRepeating (methodName, time, repeatRate) 
end

function MgrPopup:CancelInvoke () 
end

function MgrPopup:CancelInvoke (methodName) 
end

function MgrPopup:IsInvoking (methodName) 
end

function MgrPopup:IsInvoking () 
end

function MgrPopup:StartCoroutine (routine) 
end

function MgrPopup:StartCoroutine_Auto (routine) 
end

function MgrPopup:StartCoroutine (methodName, value) 
end

function MgrPopup:StartCoroutine (methodName) 
end

function MgrPopup:StopCoroutine (methodName) 
end

function MgrPopup:StopCoroutine (routine) 
end

function MgrPopup:StopCoroutine (routine) 
end

function MgrPopup:StopAllCoroutines () 
end

function MgrPopup:GetComponent (type) 
end

function MgrPopup:GetComponent () 
end

function MgrPopup:GetComponent (type) 
end

function MgrPopup:GetComponentInChildren (t, includeInactive) 
end

function MgrPopup:GetComponentInChildren (t) 
end

function MgrPopup:GetComponentInChildren () 
end

function MgrPopup:GetComponentInChildren (includeInactive) 
end

function MgrPopup:GetComponentsInChildren (t) 
end

function MgrPopup:GetComponentsInChildren (t, includeInactive) 
end

function MgrPopup:GetComponentsInChildren (includeInactive) 
end

function MgrPopup:GetComponentsInChildren (includeInactive, result) 
end

function MgrPopup:GetComponentsInChildren () 
end

function MgrPopup:GetComponentsInChildren (results) 
end

function MgrPopup:GetComponentInParent (t) 
end

function MgrPopup:GetComponentInParent () 
end

function MgrPopup:GetComponentsInParent (t) 
end

function MgrPopup:GetComponentsInParent (t, includeInactive) 
end

function MgrPopup:GetComponentsInParent (includeInactive) 
end

function MgrPopup:GetComponentsInParent (includeInactive, results) 
end

function MgrPopup:GetComponentsInParent () 
end

function MgrPopup:GetComponents (type) 
end

function MgrPopup:GetComponents (type, results) 
end

function MgrPopup:GetComponents (results) 
end

function MgrPopup:GetComponents () 
end

function MgrPopup:CompareTag (tag) 
end

function MgrPopup:SendMessageUpwards (methodName, value, options) 
end

function MgrPopup:SendMessageUpwards (methodName, value) 
end

function MgrPopup:SendMessageUpwards (methodName) 
end

function MgrPopup:SendMessageUpwards (methodName, options) 
end

function MgrPopup:SendMessage (methodName, value, options) 
end

function MgrPopup:SendMessage (methodName, value) 
end

function MgrPopup:SendMessage (methodName) 
end

function MgrPopup:SendMessage (methodName, options) 
end

function MgrPopup:BroadcastMessage (methodName, parameter, options) 
end

function MgrPopup:BroadcastMessage (methodName, parameter) 
end

function MgrPopup:BroadcastMessage (methodName) 
end

function MgrPopup:BroadcastMessage (methodName, options) 
end

function MgrPopup:ToString () 
end

function MgrPopup:GetInstanceID () 
end

function MgrPopup:GetHashCode () 
end

function MgrPopup:Equals (other) 
end

function MgrPopup:GetType () 
end

