using UnityEngine;

public class ParticleSystemSimple : MonoBehaviour
{
    [Header("Particle Settings")]
    public int maxParticles = 100;
    public float particleLifetime = 2f;
    public float particleSpeed = 5f;
    public float particleSize = 0.3f;
    public Color particleColor = Color.white;
    public bool useGravity = true;
    public float gravityScale = 9.8f;
    
    private Particle[] _particles;
    private int _particleCount = 0;
    
    [System.Serializable]
    public class Particle
    {
        public Vector3 position;
        public Vector3 velocity;
        public float lifetime;
        public float maxLifetime;
        public Color color;
        public float size;
        public bool active;
    }
    
    void Start()
    {
        _particles = new Particle[maxParticles];
        for (int i = 0; i < maxParticles; i++)
        {
            _particles[i] = new Particle();
        }
    }
    
    void Update()
    {
        for (int i = 0; i < maxParticles; i++)
        {
            if (_particles[i].active)
            {
                _particles[i].lifetime -= Time.deltaTime;
                
                // Движение
                _particles[i].position += _particles[i].velocity * Time.deltaTime;
                
                // Гравитация
                if (useGravity)
                {
                    _particles[i].velocity.y -= gravityScale * Time.deltaTime;
                }
                
                // Затухание
                float alpha = _particles[i].lifetime / _particles[i].maxLifetime;
                _particles[i].color = new Color(
                    particleColor.r,
                    particleColor.g,
                    particleColor.b,
                    alpha
                );
                
                if (_particles[i].lifetime <= 0f)
                {
                    _particles[i].active = false;
                }
            }
        }
    }
    
    void OnPostRender()
    {
        GL.PushMatrix();
        GL.LoadPixelMatrix();
        
        for (int i = 0; i < maxParticles; i++)
        {
            if (_particles[i].active)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(_particles[i].position);
                
                if (screenPos.z > 0)
                {
                    float size = _particles[i].size * _particles[i].lifetime / _particles[i].maxLifetime;
                    DrawParticle(screenPos.x, screenPos.y, size, _particles[i].color);
                }
            }
        }
        
        GL.PopMatrix();
    }
    
    void DrawParticle(float x, float y, float size, Color color)
    {
        GL.Begin(GL.QUADS);
        GL.Color(color);
        
        float halfSize = size * 50; // Масштаб для пикселей
        
        GL.Vertex3(x - halfSize, y - halfSize, 0);
        GL.Vertex3(x + halfSize, y - halfSize, 0);
        GL.Vertex3(x + halfSize, y + halfSize, 0);
        GL.Vertex3(x - halfSize, y + halfSize, 0);
        
        GL.End();
    }
    
    public void Emit(Vector3 position, Vector3 direction, int count = 10)
    {
        for (int i = 0; i < count && _particleCount < maxParticles; i++)
        {
            for (int j = 0; j < maxParticles; j++)
            {
                if (!_particles[j].active)
                {
                    _particles[j].position = position;
                    
                    // Случайное направление в конусе
                    float angle = Random.Range(-30f, 30f) * Mathf.Deg2Rad;
                    float speed = particleSpeed * Random.Range(0.8f, 1.2f);
                    
                    _particles[j].velocity = new Vector3(
                        Mathf.Sin(angle) * speed,
                        Mathf.Cos(angle) * speed,
                        0
                    );
                    
                    if (direction != Vector3.zero)
                    {
                        _particles[j].velocity += direction.normalized * particleSpeed;
                    }
                    
                    _particles[j].lifetime = particleLifetime;
                    _particles[j].maxLifetime = particleLifetime;
                    _particles[j].color = particleColor;
                    _particles[j].size = particleSize;
                    _particles[j].active = true;
                    
                    _particleCount++;
                    break;
                }
            }
        }
    }
    
    public void StopAllParticles()
    {
        for (int i = 0; i < maxParticles; i++)
        {
            _particles[i].active = false;
        }
        _particleCount = 0;
    }
}
