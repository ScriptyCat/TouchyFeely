using IllusionPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace TouchyFeely {
    /// <summary>
    /// This is a test plugin that you can use a reference.
    /// Notes:
    ///     - Compile for .NET 2.0 or .NET 3.5
    ///     - Change the namespace and the class name
    ///     - Add references to the DLLs you need in the "References" folder
    ///         - UnityEngine.dll - Unity classes, you'll always want this one
    ///         - Assembly-CSharp-PlayClub.dll - Classes specific to PlayClub, you'll probably also want this one
    ///         - IllusionPlugin.dll - Contains the IPlugin interface. Required.
    ///         - UnityEngine.UI.dll - Unity classes for UI stuff. Might be needed.
    ///     - Compiling this project with the "Release" configuratoin will copy the files into the "Dist" folder.
    /// </summary>
    public class Touchy : IPlugin {

        public string Name {
            get { return "TouchyFeely"; }
        }

        public string Version {
            get { return "0.0.0"; }
        }

        public void OnApplicationStart() {
            Console.WriteLine("Oh god, I was loaded!");
            ReadConfig();
        }

        string GetPathName() {
            string path = Application.dataPath;
            path = path.Remove(path.LastIndexOf("/"));
            path += "/Plugins";
            return path;
        }

        string pluginPath;
        string hasValues = null;
        string[] configVars;
        KeyCode[] useKeys;
        //KeyCode[] lfKeys;
        //KeyCode[] rfKeys;
        bool toggle;
        bool realTime = false;
        bool defState;
        bool useButt = true;
        float sphereScale;
        Vector3 sphereSize;
        float buttElas;
        float buttStif;
        float buttDamp;
        float buttRad;
        float buttScale;
        float mouseSpeed;
        float mouseScale;
        float mouseDepth;
        bool mouseDebug;
        bool boobDebug;
        void ReadConfig() {
            pluginPath = GetPathName();
            string[] config = File.ReadAllLines(pluginPath + "/TouchyConfig.ini");
            List<string> varLines = new List<string>();
            foreach (string str in config) {
                if (str[0].ToString() == "/")
                    continue;
                varLines.Add(str);
            }
            configVars = varLines.ToArray();

            useKeys = GetKeys(GetCVar("UseKey"));
            //lfKeys = GetKeys(GetCVar("LeftBoobFreezeToggle"));
            //rfKeys = GetKeys(GetCVar("RightBoobFreezeToggle"));

            if (!bool.TryParse(GetCVar("Toggle"), out toggle)) { toggle = true; }
            if (!bool.TryParse(GetCVar("DefaultState"), out defState)) { defState = true; }
            if (!bool.TryParse(GetCVar("UseButtPhysics"), out useButt)) { useButt = true; }
            if (!realTime)
                isOn = defState;
            if (!bool.TryParse(GetCVar("UpdateRealtime"), out realTime)) { realTime = false; }
            if (!float.TryParse(GetCVar("SphereScalefactor"), out sphereScale)) { sphereScale = 1.95f; }
            if (!float.TryParse(GetCVar("ButtStiffness"), out buttStif)) { buttStif = 0.3f; }
            if (!float.TryParse(GetCVar("ButtElasticity"), out buttElas)) { buttElas = 0.3f; }
            if (!float.TryParse(GetCVar("ButtDamping"), out buttDamp)) { buttDamp = 0.3f; }
            if (!float.TryParse(GetCVar("ButtColliderRadius"), out buttRad)) { buttRad = 0.3f; }
            if (!float.TryParse(GetCVar("ButColliderScale"), out buttScale)) { buttScale = 0.3f; }
            if (!float.TryParse(GetCVar("SphereSizeX"), out sphereSize.x)) { sphereSize.x = 0.02f; }
            if (!float.TryParse(GetCVar("SphereSizeY"), out sphereSize.y)) { sphereSize.y = 0.02f; }
            if (!float.TryParse(GetCVar("SphereSizeZ"), out sphereSize.z)) { sphereSize.z = 0f; }
            if (!float.TryParse(GetCVar("MouseCollSpeed"), out mouseSpeed)) { mouseSpeed = 14f; }
            if (!float.TryParse(GetCVar("MouseCollScale"), out mouseScale)) { mouseScale = 0.09f; }
            if (!float.TryParse(GetCVar("MouseCollDepth"), out mouseDepth)) { mouseDepth = 0.01f; }
            if (!bool.TryParse(GetCVar("BreastCollDebug"), out boobDebug)) { boobDebug = false; }
            if (!bool.TryParse(GetCVar("MouseCollDebug"), out mouseDebug)) { mouseDebug = false; }


            hasValues = "yes";
        }

        KeyCode[] GetKeys(string keyString) {
            List<KeyCode> keyLst = new List<KeyCode>();
            string keyStr = keyString;
            if (keyStr.IndexOf("+") != -1) {
                while (keyStr.IndexOf("+") != -1) {
                    keyLst.Add((KeyCode)Enum.Parse(typeof(KeyCode), keyStr.Substring(0, keyStr.IndexOf("+")), true));
                    keyStr = keyStr.Substring(keyStr.IndexOf("+") + 1);

                }

                keyLst.Add((KeyCode)Enum.Parse(typeof(KeyCode), keyStr, true));
            } else {
                keyLst.Add((KeyCode)Enum.Parse(typeof(KeyCode), keyStr, true));
            }

            return keyLst.ToArray();
        }

        string GetCVar(string str) {
            string toGet = null;
            foreach (string ctr in configVars) {
                if (ctr.IndexOf(str) != -1)
                    toGet = ctr;
            }
            if (toGet == null)
                return null;

            toGet = toGet.Substring(toGet.IndexOf("=") + 1);

            while (toGet[0].ToString() == " ")
                toGet = toGet.Remove(0, 1);

            return toGet;
        }

        bool WereKeysPressed(KeyCode[] keysToUse) {
            for (int i = 0; i < keysToUse.Length; i++)
                if (!Input.GetKey(keysToUse[i]))
                    return false;
            bool kpc = false;
            for (int i = 0; i < keysToUse.Length; i++)
                if (Input.GetKeyDown(keysToUse[i]))
                    kpc = true;
            return kpc;
        }

        bool WereKeysHeld(KeyCode[] keysToUse) {
            for (int i = 0; i < keysToUse.Length; i++)
                if (!Input.GetKey(keysToUse[i]))
                    return false;
            return true;
        }

        /// <param name="level"></param>
        public void OnLevelWasLoaded(int level) {
            var hscene = GameObject.FindObjectOfType<H_Scene>();
            if (hscene != null) {

                // hscene.ChangeMap
                // hscene.ChangeState
                // etc.
            }
        }

        /// <param name="level"></param>
        public void OnLevelWasInitialized(int level) {
        }

        //string prevHit = "";
        GameObject[] feelNodes;
        bool isTouching = false;
        bool isOn = false;
        Human human;
        Human prevHuman;        
        int timeOut = 0;
        int searchInt = 0;
        public void OnUpdate() {
            if (hasValues == null)
                ReadConfig();

            if (realTime)
                ReadConfig();

            if (toggle) {
                if (WereKeysPressed(useKeys))
                    isOn = !isOn;
            } else {
                isOn = false;
                if (WereKeysHeld(useKeys))
                    isOn = true;
            }

            if (useButt) {
                searchInt++;
                if (searchInt > 14) {
                    searchInt = 0;
                    CheckButtPhysics();
                }
            }

            if (isOn) {
                if (!isTouching) {
                    //create
                    Ray rayy = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitt;

                    if (Physics.Raycast(rayy, out hitt, 20)) {
                        human = hitt.collider.GetComponentInParent<Human>();
                        prevHuman = human;
                        if (!human)
                            return;
                        if (human.sex != Human.SEX.FEMALE)
                            return;

                        //File.WriteAllLines(Application.dataPath + "/hit.txt", new string[] { human.transform.name });
                    } else {
                        return;
                    }

                    if (useButt) {
                        DestroyButt(human);
                        CheckButtPhysics();
                    }

                    Transform bust = human.transform.Find("DynamicBone/DynamicBone_Bust").transform;
                    DynamicBone_Custom[] dbs = bust.GetComponentsInChildren<DynamicBone_Custom>();

                    isTouching = true;

                    List<GameObject> tmpgos = new List<GameObject>();
                    List<string> dbgs = new List<string>();
                    for (int p = 0; p < 2; p++) {
                        //nodevisualization
                        /*
                        dbgs.Add(dbs.Length.ToString());
                        dbgs.Add(" ");
                        dbgs.Add(" - dbs[p] " + p.ToString());
                        dbgs.Add("force: " + dbs[p].m_Force.x.ToString() + ", " + dbs[p].m_Force.y.ToString() + ", " + dbs[p].m_Force.z.ToString());
                        dbgs.Add("stiff: " + dbs[p].m_Stiffness.ToString());
                        dbgs.Add("elas: " + dbs[p].m_Elasticity.ToString());
                        dbgs.Add("damp: " + dbs[p].m_Damping.ToString());
                        dbgs.Add(" ");
                        */
                        for (int i = 0; i < 2; i++) {
                            GameObject tmpSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            float rr = dbs[p].m_Radius * sphereScale;
                            tmpSphere.transform.localScale = new Vector3(rr + sphereSize.x, rr + sphereSize.y, rr + sphereSize.z);
                            tmpSphere.transform.rotation = dbs[p].m_Nodes.ToArray()[i].rotation;
                            tmpSphere.transform.SetParent(dbs[p].m_Nodes.ToArray()[i]);
                            tmpSphere.transform.position = dbs[p].m_Nodes.ToArray()[i].position;
                            if (!boobDebug) {
                                GameObject.Destroy(tmpSphere.GetComponent<MeshRenderer>());
                                GameObject.Destroy(tmpSphere.GetComponent<MeshFilter>());
                            }
                            tmpSphere.GetComponent<Collider>().isTrigger = true;
                            tmpgos.Add(tmpSphere);
                        }
                    }
                    //File.WriteAllLines(Application.dataPath + "/andbg.txt", dbgs.ToArray());

                    feelBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    GameObject.Destroy(feelBall.GetComponent<Collider>());
                    if (!mouseDebug) {
                        GameObject.Destroy(feelBall.GetComponent<MeshRenderer>());
                        GameObject.Destroy(feelBall.GetComponent<MeshFilter>());
                    }
                    feelBallColl = feelBall.AddComponent<DynamicBoneCollider>();
                    feelBallColl.m_Radius = 0.5f;
                    feelBallColl.m_Height = 0.0f;
                    feelBall.transform.localScale = new Vector3(mouseScale, mouseScale, mouseScale);
                    feelBall.transform.position = human.transform.position;

                    for (int i = 0; i < dbs.Length; i++) {
                        dbs[i].m_Colliders.Add(feelBallColl);
                    }
                    feelNodes = tmpgos.ToArray();
                    //File.WriteAllLines(Application.dataPath + "/node.txt", new string[] { " " });
                } else {
                    //if we are touching
                    if (feelBall) {
                        Ray rayy = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hitt;

                        if (Physics.Raycast(rayy, out hitt, 20)) {
                            feelBallColl.enabled = true;                            
                            human = hitt.collider.GetComponentInParent<Human>();
                            if (human != prevHuman) {
                                DestroyCurrentFeel();
                                return;
                            }
                            timeOut = 0;
                            float depth = mouseScale / 2f - mouseDepth;
                            feelBall.transform.position = Vector3.Lerp(feelBall.transform.position, hitt.point + hitt.normal * depth, Time.deltaTime * 14f);
                            //File.WriteAllLines(Application.dataPath + "/hit.txt", new string[] { mainParent.ToString() });
                        } else {
                            timeOut++;
                            feelBallColl.enabled = false;
                            if (timeOut > 175) {
                                DestroyCurrentFeel();
                                return;
                            }
                        }

                        /*
                        if (WereKeysPressed(lfKeys)) {
                            
                        }
                        if (WereKeysPressed(rfKeys)) {
                           
                        }
                        */
                    }
                }
            } else {
                DestroyCurrentFeel();
            }

            /*
            if (Input.GetKey(KeyCode.X)) {
                List<string> dbgs = new List<string>();
                Transform[] boobTrans = GameObject.Find("cf_J_Mune00").GetComponentsInChildren<Transform>();
                foreach (Transform trn in boobTrans) {
                    if (trn.localScale != Vector3.one && trn.name != "LiquidCollider")
                        dbgs.Add(trn.name + " : (" + trn.localScale.x + ", " + trn.localScale.y + ", " + trn.localScale.z + ")");
                }
                //File.WriteAllLines(Application.dataPath + "/transdbg.txt", dbgs.ToArray());
            }
            */

            //end method
        }
        GameObject feelBall;
        DynamicBoneCollider feelBallColl;
        GameObject blgo;
        GameObject brgo;

        void DestroyCurrentFeel() {
            if (isTouching) {
                //destroy
                isTouching = false;
                if (feelNodes != null) {
                    foreach (GameObject go in feelNodes) {
                        GameObject.Destroy(go);
                    }
                }
                if (feelBall)
                    GameObject.Destroy(feelBall);
            }
        }

        void CheckButtPhysics() {
            Human[] humns = UnityEngine.Object.FindObjectsOfType<Human>();
            foreach (Human humn in humns) {
                if (humn.sex == Human.SEX.FEMALE) {
                    if (humn.transform.Find("DynamicBone/DynamicBone_Bust/buttLeft") == null) {
                        Transform bust = humn.transform.Find("DynamicBone/DynamicBone_Bust");
                        blgo = GameObject.Instantiate(bust.transform.GetChild(0).gameObject) as GameObject;
                        brgo = GameObject.Instantiate(bust.transform.GetChild(1).gameObject) as GameObject;
                        blgo.transform.SetParent(bust); brgo.transform.SetParent(bust);
                        blgo.name = "buttLeft"; brgo.name = "buttRight";
                        blgo.transform.localPosition = Vector3.zero; brgo.transform.localPosition = Vector3.zero;
                        blgo.transform.localEulerAngles = Vector3.zero; brgo.transform.localEulerAngles = Vector3.zero;
                        Transform[] trans = human.transform.Find("cf_body_01").GetComponentsInChildren<Transform>();
                        DynamicBone_Custom buttL;
                        DynamicBone_Custom buttR;
                        foreach (Transform tr in trans) {
                            if (tr.name == "cf_J_Siri_hit_L") {
                                buttL = blgo.GetComponent<DynamicBone_Custom>();
                                buttL.m_Radius = buttRad;
                                buttL.m_Nodes.Clear();
                                buttL.m_Nodes.Add(tr.parent);
                                buttL.m_Nodes.Add(tr);
                                buttL.m_Colliders.Clear();
                                buttL.m_Colliders.Add(feelBallColl);
                                buttL.m_Stiffness = 0.3f;
                                buttL.m_Elasticity = 0.3f;
                                buttL.m_Damping = 0.3f;
                                GameObject tmpSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                float rr = buttL.m_Radius * buttScale;
                                tmpSphere.transform.localScale = new Vector3(rr + sphereSize.x, rr + sphereSize.y, rr + sphereSize.z);
                                tmpSphere.transform.SetParent(tr.parent.parent);
                                tmpSphere.transform.localPosition = Vector3.zero;
                                if (!boobDebug) {
                                    GameObject.Destroy(tmpSphere.GetComponent<MeshRenderer>());
                                    GameObject.Destroy(tmpSphere.GetComponent<MeshFilter>());
                                }
                                tmpSphere.GetComponent<Collider>().isTrigger = true;
                            }
                            if (tr.name == "cf_J_Siri_hit_R") {
                                buttR = brgo.GetComponent<DynamicBone_Custom>();
                                buttR.m_Radius = buttRad;
                                buttR.m_Nodes.Clear();
                                buttR.m_Nodes.Add(tr.parent);
                                buttR.m_Nodes.Add(tr);
                                buttR.m_Colliders.Clear();
                                buttR.m_Colliders.Add(feelBallColl);
                                buttR.m_Stiffness = buttStif;
                                buttR.m_Elasticity = buttElas;
                                buttR.m_Damping = buttDamp;
                                GameObject tmpSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                float rr = buttR.m_Radius * buttScale;
                                tmpSphere.transform.localScale = new Vector3(rr + sphereSize.x, rr + sphereSize.y, rr + sphereSize.z);
                                tmpSphere.transform.SetParent(tr.parent.parent);
                                tmpSphere.transform.localPosition = Vector3.zero;
                                if (!boobDebug) {
                                    GameObject.Destroy(tmpSphere.GetComponent<MeshRenderer>());
                                    GameObject.Destroy(tmpSphere.GetComponent<MeshFilter>());
                                }
                                tmpSphere.GetComponent<Collider>().isTrigger = true;
                            }
                        }
                        
                    }
                }
            }
        }

        void DestroyButt(Human humn) {
            Transform bust = humn.transform.Find("DynamicBone/DynamicBone_Bust");
            if (bust.Find("buttLeft")) {
                GameObject.Destroy(bust.Find("buttLeft").gameObject);
            }
            if (bust.Find("buttRight")) {
                GameObject.Destroy(bust.Find("buttRight").gameObject);
            }
        }

        public void OnFixedUpdate() {
        }

        public void OnApplicationQuit() {
        }

    }
}
