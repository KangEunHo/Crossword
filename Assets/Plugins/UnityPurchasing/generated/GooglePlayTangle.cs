#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("8e66EYc7gCV9XuI1bgMB2stRgcEVoHnG4GCJCxFl+3zbY2W26Ub5qdKyfzy6A40RmjqvHplw9LgAgVylC7RH718IpSdmymlKzLxzH4T88yEvrKKtnS+sp68vrKytTuk/ZG3WE/xUmcalDmUIIpBfL65I0aDAy9egnS+sj52gq6SHK+UrWqCsrKyora5jWAzbgnaduplcXXXkfnE+1CokgXcqFBtPtHC2ZdR1NP3nn1iGS/qf2OUCp8bPbxXFgr9RsDymNCEKDPPdJf2UK4mOXfDvku0ZvmZzvdaOFlnpjHPJbVKcHPNCCFolnndWJHGtMekXe1+8VqLhOEdqVEU9F70cp11paNS8cakYzhBUD7fc4FUjNuDM/EHrV3veTMTFJK+urK2s");
        private static int[] order = new int[] { 3,9,8,10,9,5,10,9,8,12,13,12,12,13,14 };
        private static int key = 173;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
