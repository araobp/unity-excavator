using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlanetController : MonoBehaviour
{
    // 惑星の初速
    // Initial speed of the planet
    [SerializeField] float initialSpeed = 20F;
    // X軸に対する軌道の傾き
    // Orbital inclination with respect to the X-axis
    [SerializeField] float orbitalInclination = 0F;

    // 惑星が周回する恒星のGameObject
    // GameObject of the star the planet orbits
    GameObject star;
    // 恒星の質量
    // Mass of the star
    float starMass;
    // 惑星の質量
    // Mass of the planet
    float planetMass;
    // 惑星のRigidbodyコンポーネント
    // Rigidbody component of the planet
    Rigidbody rb;

    // 万有引力定数。シミュレーションのために実際の値より10^11倍大きくしています。
    // The gravitational constant. It's 10^11 times larger than the real value for simulation purposes.
    const float G = 6.674F;  // This is 10^11 times larger than the real constant of gravitatioin. 

    // Start is called before the first frame update
    void Start()
    {
        // シーン内の"Star"という名前のGameObjectを検索して取得します。
        // Find and get the GameObject named "Star" in the scene.
        star = GameObject.Find("Star");
        // 恒星のRigidbodyコンポーネントから質量を取得します。
        // Get the mass from the star's Rigidbody component.
        starMass = star.GetComponent<Rigidbody>().mass;
        // この惑星のRigidbodyコンポーネントを取得します。
        // Get this planet's Rigidbody component.
        rb = gameObject.GetComponent<Rigidbody>();
        // この惑星の質量を取得します。
        // Get the mass of this planet.
        planetMass = rb.mass;
        // x軸を軸に軌道を傾けます。
        // Tilt the orbit on the x-axis.
        transform.Rotate(orbitalInclination, 0, 0);
        // 回転後の初期の前方ベクトルを取得します。
        // Get the initial forward vector after rotation.
        Vector3 initialForward = new Vector3(rb.transform.forward.x, rb.transform.forward.y, rb.transform.forward.z);
        // 惑星の初速を設定します。
        // Set the initial velocity of the planet.
        rb.linearVelocity = new Vector3(initialForward.x, initialForward.y, initialForward.z) * initialSpeed;

        // "ButtonClose"を見つけ、クリックされたときにメニューシーンに戻るリスナーを追加します。
        // Find "ButtonClose" and add a listener to return to the Menu scene when clicked.
        GameObject.Find("ButtonClose").GetComponent<Button>().onClick.AddListener(
            delegate
            {
                GoHome();
            }
        );
    }

    // FixedUpdateは固定間隔で呼び出され、物理計算に使用されます。
    // FixedUpdate is called at fixed intervals and is used for physics calculations.
    void FixedUpdate()
    {
        // 惑星から恒星へのベクトルを計算します。
        // Calculate the vector from the planet to the star.
        Vector3 r = star.transform.position - transform.position;
        // ニュートンの万有引力の法則を用いて重力を計算します。
        // Calculate gravity using Newton's law of universal gravitation.
        float gravity = G * starMass * planetMass / Mathf.Pow(r.magnitude, 2F);
        // 惑星のRigidbodyに重力を適用します。
        // Apply gravity to the planet's Rigidbody.
        rb.AddForce(gravity * r.normalized, ForceMode.Force);
    }

    // "Menu"シーンをロードします。
    // Loads the "Menu" scene.
    private void GoHome()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }
}
